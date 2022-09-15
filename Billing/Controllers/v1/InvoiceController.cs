using AutoMapper;
using Billing.Business.Contracts;
using Billing.Data.EF.Contracts;
using Billing.Data.Entities;
using Billing.Helpers;
using Billing.Helpers.ResourceParameters;
using Billing.Models;
using Billing.Models.Invoice;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Billing.API.Controllers.v1
{
	[ApiController]
	[ApiVersion("1.0")]
	[Route("api/v1/[controller]")]
	public class InvoiceController : ControllerBase
	{
		private readonly IPropertyMappingService _propertyMappingService;
		private readonly IPropertyCheckerService _propertyCheckerService;
		private readonly IInvoiceRepo _invoiceRepo;
		private readonly IUrlHelper _urlHelper;
		private readonly IMapper _mapper;

		public InvoiceController(IInvoiceRepo invoiceRepo,
							 IUrlHelper urlHelper,
							 IMapper mapper,
							 IPropertyMappingService propertyMappingService,
							 IPropertyCheckerService propertyCheckerService)
		{
			_invoiceRepo = invoiceRepo;
			_urlHelper = urlHelper;
			_mapper = mapper;
			_propertyMappingService = propertyMappingService;
			_propertyCheckerService = propertyCheckerService;
		}

		[HttpGet(Name = "GetInvoice")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<IEnumerable<InvoiceModel>>> GetInvoice([FromQuery] InvoiceResourceParameters InvoiceResourceParameters)
		{
			if (!_propertyMappingService.ValidMappingExistsFor<InvoiceModel, Invoice>
				(InvoiceResourceParameters.OrderBy))
			{
				return BadRequest();
			}

			if (!_propertyCheckerService.TypeHasProperties<InvoiceModel>
				(InvoiceResourceParameters.Fields))
			{
				return BadRequest();
			}

			var InvoiceFromDatabase = await _invoiceRepo.GetInvoiceAsync(InvoiceResourceParameters);

			var previousPageLink = InvoiceFromDatabase.HasPrevious ?
			   CreateInvoiceResourceUri(InvoiceResourceParameters,
			   ResourceUriType.PreviousPage) : null;

			var nextPageLink = InvoiceFromDatabase.HasNext ?
				CreateInvoiceResourceUri(InvoiceResourceParameters,
				ResourceUriType.NextPage) : null;

			var paginationMetadata = new
			{
				totalCount = InvoiceFromDatabase.TotalCount,
				pageSize = InvoiceFromDatabase.PageSize,
				currentPage = InvoiceFromDatabase.CurrentPage,
				totalPages = InvoiceFromDatabase.TotalPages,
				previousPageUri = previousPageLink,
				nextPageUri = nextPageLink
			};

			Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

			var InvoiceToReturn = _mapper.Map<IEnumerable<InvoiceModel>>(InvoiceFromDatabase);

			return Ok(InvoiceToReturn.ShapeData(InvoiceResourceParameters.Fields));
		}

		[HttpGet("{id}", Name = "GetSingleInvoice")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<InvoiceModel>> GetSingleInvoice(int id, [FromQuery] string? fields)
		{
			if (!_propertyCheckerService.TypeHasProperties<InvoiceModel>(fields))
			{
				return BadRequest();
			}

			var InvoiceFromDatabase = await _invoiceRepo.GetInvoiceAsync(id);

			if (InvoiceFromDatabase == null)
			{
				return NotFound();
			}

			var InvoiceToReturn = _mapper.Map<InvoiceModel>(InvoiceFromDatabase);
			return Ok(InvoiceToReturn.ShapeData(fields));
		}

		[HttpPost(Name = "CreateInvoice")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<InvoiceModel>> CreateInvoice([FromBody] InvoiceForCreationModel Invoice)
		{
			var InvoiceEntity = _mapper.Map<Invoice>(Invoice);
			await _invoiceRepo.AddInvoiceAsync(InvoiceEntity);
			await _invoiceRepo.SaveAsync(); //TODO: check for errors

			var InvoiceToReturn = _mapper.Map<InvoiceModel>(InvoiceEntity);

			return CreatedAtRoute(nameof(GetSingleInvoice), new { id = InvoiceToReturn.Id }, InvoiceToReturn);
		}

		[HttpOptions]
		public IActionResult GetInvoiceOptions()
		{
			Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,PATCH,DELETE");
			return Ok();
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> UpdateInvoice(int id, InvoiceForUpdateModel Invoice)
		{
			var InvoiceFromDatabase = await _invoiceRepo.GetInvoiceAsync(id);

			if (InvoiceFromDatabase == null)
			{
				var InvoiceToAdd = _mapper.Map<Invoice>(Invoice);
				InvoiceToAdd.Id = id;

				await _invoiceRepo.AddInvoiceAsync(InvoiceToAdd);
				await _invoiceRepo.SaveAsync();

				var InvoiceToReturn = _mapper.Map<InvoiceModel>(InvoiceToAdd);

				return CreatedAtRoute(nameof(GetSingleInvoice), new { id = InvoiceToReturn.Id }, InvoiceToReturn);
			}

			_mapper.Map(Invoice, InvoiceFromDatabase);
			_invoiceRepo.UpdateInvoice(InvoiceFromDatabase);
			await _invoiceRepo.SaveAsync();

			return NoContent();
		}

		[HttpPatch("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult> PartiallyUpdateInvoice(int id, JsonPatchDocument<InvoiceForUpdateModel> patchDocument)
		{
			var InvoiceFromDatabase = await _invoiceRepo.GetInvoiceAsync(id);

			if (InvoiceFromDatabase == null)
			{
				var Invoice = new InvoiceForUpdateModel();
				patchDocument.ApplyTo(Invoice);//TODO: check if ModelState needs to be passed

				if (!TryValidateModel(Invoice))
				{
					return ValidationProblem(ModelState);
				}

				var InvoiceToAdd = _mapper.Map<Invoice>(Invoice);
				InvoiceToAdd.Id = id;

				await _invoiceRepo.AddInvoiceAsync(InvoiceToAdd);
				await _invoiceRepo.SaveAsync();

				var InvoiceToReturn = _mapper.Map<InvoiceModel>(InvoiceToAdd);

				return CreatedAtRoute(nameof(GetSingleInvoice), new { id = InvoiceToReturn.Id }, InvoiceToReturn);
			}

			var InvoiceToPatch = _mapper.Map<InvoiceForUpdateModel>(InvoiceFromDatabase);
			patchDocument.ApplyTo(InvoiceToPatch);

			if (!TryValidateModel(InvoiceToPatch))
			{
				return ValidationProblem(ModelState);
			}

			_mapper.Map(InvoiceToPatch, InvoiceFromDatabase);

			_invoiceRepo.UpdateInvoice(InvoiceFromDatabase);
			await _invoiceRepo.SaveAsync();

			return NoContent();
		}

		[HttpDelete("{id}", Name = ("DeleteInvoice"))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> DeleteInvoice(int id)
		{
			var InvoiceFromDatabase = await _invoiceRepo.GetInvoiceAsync(id);

			if (InvoiceFromDatabase == null)
			{
				return NotFound();
			}

			_invoiceRepo.DeleteInvoice(InvoiceFromDatabase);
			await _invoiceRepo.SaveAsync();

			return NoContent();
		}

		public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
		{
			var options = HttpContext.RequestServices
				.GetRequiredService<IOptions<ApiBehaviorOptions>>();
			return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
		}


		private string CreateInvoiceResourceUri(
			InvoiceResourceParameters resourceParameters,
			ResourceUriType type)
		{
			switch (type)
			{
				case ResourceUriType.PreviousPage:
					return _urlHelper.Link("GetInvoice",
						new
						{
							orderBy = resourceParameters.OrderBy,
							fields = resourceParameters.Fields,
							searchQuery = resourceParameters.SearchQuery!,
							pageNumber = resourceParameters.PageNumber - 1,
							pageSize = resourceParameters.PageSize
						}
					);
				case ResourceUriType.NextPage:
					return _urlHelper.Link("GetInvoice",
						new
						{
							orderBy = resourceParameters.OrderBy,
							fields = resourceParameters.Fields,
							searchQuery = resourceParameters.SearchQuery,
							pageNumber = resourceParameters.PageNumber + 1,
							pageSize = resourceParameters.PageSize
						}
					);
				case ResourceUriType.Current:
				default:
					return _urlHelper.Link("GetInvoice",
						new
						{
							orderBy = resourceParameters.OrderBy,
							fields = resourceParameters.Fields,
							searchQuery = resourceParameters.SearchQuery,
							pageNumber = resourceParameters.PageNumber,
							pageSize = resourceParameters.PageSize
						}
					);
			}
		}

	}
}
