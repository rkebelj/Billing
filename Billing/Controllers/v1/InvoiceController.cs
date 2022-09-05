using AutoMapper;
using Billing.Business.Contracts;
using Billing.Data.EF.Contracts;
using Billing.Data.Entities;
using Billing.Helpers;
using Billing.Helpers.ResourceParameters;
using Billing.Models;
using Billing.Models.Bill;
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
		private readonly IInvoiceRepo _billRepo;
		private readonly IUrlHelper _urlHelper;
		private readonly IMapper _mapper;

		public InvoiceController(IInvoiceRepo billRepo,
							 IUrlHelper urlHelper,
							 IMapper mapper,
							 IPropertyMappingService propertyMappingService,
							 IPropertyCheckerService propertyCheckerService)
		{
			_billRepo = billRepo;
			_urlHelper = urlHelper;
			_mapper = mapper;
			_propertyMappingService = propertyMappingService;
			_propertyCheckerService = propertyCheckerService;
		}

		[HttpGet(Name = "GetBill")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<IEnumerable<InvoiceModel>>> GetBill([FromQuery] InvoiceResourceParameters BillResourceParameters)
		{
			if (!_propertyMappingService.ValidMappingExistsFor<InvoiceModel, Invoice>
				(BillResourceParameters.OrderBy))
			{
				return BadRequest();
			}

			if (!_propertyCheckerService.TypeHasProperties<InvoiceModel>
				(BillResourceParameters.Fields))
			{
				return BadRequest();
			}

			var BillFromDatabase = await _billRepo.GetBillAsync(BillResourceParameters);

			var previousPageLink = BillFromDatabase.HasPrevious ?
			   CreateBillResourceUri(BillResourceParameters,
			   ResourceUriType.PreviousPage) : null;

			var nextPageLink = BillFromDatabase.HasNext ?
				CreateBillResourceUri(BillResourceParameters,
				ResourceUriType.NextPage) : null;

			var paginationMetadata = new
			{
				totalCount = BillFromDatabase.TotalCount,
				pageSize = BillFromDatabase.PageSize,
				currentPage = BillFromDatabase.CurrentPage,
				totalPages = BillFromDatabase.TotalPages,
				previousPageUri = previousPageLink,
				nextPageUri = nextPageLink
			};

			Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

			var BillToReturn = _mapper.Map<IEnumerable<InvoiceModel>>(BillFromDatabase);

			return Ok(BillToReturn.ShapeData(BillResourceParameters.Fields));
		}

		[HttpGet("{id}", Name = "GetSingleBill")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult<InvoiceModel>> GetSingleBill(int id, [FromQuery] string? fields)
		{
			if (!_propertyCheckerService.TypeHasProperties<InvoiceModel>(fields))
			{
				return BadRequest();
			}

			var BillFromDatabase = await _billRepo.GetBillAsync(id);

			if (BillFromDatabase == null)
			{
				return NotFound();
			}

			var BillToReturn = _mapper.Map<InvoiceModel>(BillFromDatabase);
			return Ok(BillToReturn.ShapeData(fields));
		}

		[HttpPost(Name = "CreateBill")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult<InvoiceModel>> CreateBill([FromBody] InvoiceForCreationModel  Bill)
		{
			var BillEntity = _mapper.Map<Invoice>(Bill);
			await _billRepo.AddBillAsync(BillEntity);
			await _billRepo.SaveAsync(); //TODO: check for errors

			var BillToReturn = _mapper.Map<InvoiceModel>(BillEntity);

			return CreatedAtRoute(nameof(GetSingleBill), new { id = BillToReturn.Id }, BillToReturn);
		}

		[HttpOptions]
		public IActionResult GetBillOptions()
		{
			Response.Headers.Add("Allow", "GET,OPTIONS,POST,PUT,PATCH,DELETE");
			return Ok();
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesDefaultResponseType]
		public async Task<IActionResult> UpdateBill(int id, InvoiceForUpdateModel Bill)
		{
			var BillFromDatabase = await _billRepo.GetBillAsync(id);

			if (BillFromDatabase == null)
			{
				var BillToAdd = _mapper.Map<Invoice>(Bill);
				BillToAdd.Id = id;

				await _billRepo.AddBillAsync(BillToAdd);
				await _billRepo.SaveAsync();

				var BillToReturn = _mapper.Map<InvoiceModel>(BillToAdd);

				return CreatedAtRoute(nameof(GetSingleBill), new { id = BillToReturn.Id }, BillToReturn);
			}

			_mapper.Map(Bill, BillFromDatabase);
			_billRepo.UpdateBill(BillFromDatabase);
			await _billRepo.SaveAsync();

			return NoContent();
		}

		[HttpPatch("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		public async Task<ActionResult> PartiallyUpdateBill(int id, JsonPatchDocument<InvoiceForUpdateModel> patchDocument)
		{
			var BillFromDatabase = await _billRepo.GetBillAsync(id);

			if (BillFromDatabase == null)
			{
				var Bill = new InvoiceForUpdateModel();
				patchDocument.ApplyTo(Bill);//TODO: check if ModelState needs to be passed

				if (!TryValidateModel(Bill))
				{
					return ValidationProblem(ModelState);
				}

				var BillToAdd = _mapper.Map<Invoice>(Bill);
				BillToAdd.Id = id;

				await _billRepo.AddBillAsync(BillToAdd);
				await _billRepo.SaveAsync();

				var BillToReturn = _mapper.Map<InvoiceModel>(BillToAdd);

				return CreatedAtRoute(nameof(GetSingleBill), new { id = BillToReturn.Id }, BillToReturn);
			}

			var BillToPatch = _mapper.Map<InvoiceForUpdateModel>(BillFromDatabase);
			patchDocument.ApplyTo(BillToPatch);

			if (!TryValidateModel(BillToPatch))
			{
				return ValidationProblem(ModelState);
			}

			_mapper.Map(BillToPatch, BillFromDatabase);

			_billRepo.UpdateBill(BillFromDatabase);
			await _billRepo.SaveAsync();

			return NoContent();
		}

		[HttpDelete("{id}", Name = ("DeleteBill"))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesDefaultResponseType]
		public async Task<ActionResult> DeleteBill(int id)
		{
			var BillFromDatabase = await _billRepo.GetBillAsync(id);

			if (BillFromDatabase == null)
			{
				return NotFound();
			}

			_billRepo.DeleteBill(BillFromDatabase);
			await _billRepo.SaveAsync();

			return NoContent();
		}

		public override ActionResult ValidationProblem([ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
		{
			var options = HttpContext.RequestServices
				.GetRequiredService<IOptions<ApiBehaviorOptions>>();
			return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
		}


		private string CreateBillResourceUri(
			InvoiceResourceParameters resourceParameters,
			ResourceUriType type)
		{
			switch (type)
			{
				case ResourceUriType.PreviousPage:
					return _urlHelper.Link("GetBill",
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
					return _urlHelper.Link("GetBill",
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
					return _urlHelper.Link("GetBill",
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
