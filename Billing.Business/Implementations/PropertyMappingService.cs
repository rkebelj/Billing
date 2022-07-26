﻿using Billing.Business.Contracts;
using Billing.Data.Entities;
using Billing.Models;
using Billing.Models.Invoice;
using Billing.Models.Material;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Business.Implementations
{
	public class PropertyMappingService : IPropertyMappingService
	{
		private Dictionary<string, PropertyMappingValue> _invoiceMapping =
			new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
			{
				{ "Id", new PropertyMappingValue(new List<string>() { "Id" }) },
				{ "CustomerId", new PropertyMappingValue(new List<string>() { "CustomerId" }) },
				{ "Date", new PropertyMappingValue(new List<string>() { "Date" }) },
				{ "DDV", new PropertyMappingValue(new List<string>() { "DDV" }) },
				{ "Discount", new PropertyMappingValue(new List<string>() { "Discount" }) },
			};
		private Dictionary<string, PropertyMappingValue> _materialMapping =
			new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
			{
				{ "Id", new PropertyMappingValue(new List<string>() { "Id" }) },
				{ "CategoryId", new PropertyMappingValue(new List<string>() { "CategoryId" }) },
				{ "Name", new PropertyMappingValue(new List<string>() { "Name" }) },
				{ "Price", new PropertyMappingValue(new List<string>() { "Price" }) },
				{ "Unit", new PropertyMappingValue(new List<string>() { "Unit" }) },
				{ "Comment", new PropertyMappingValue(new List<string>() { "Comment" }) },
			};
		private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

		public PropertyMappingService()
		{
			_propertyMappings.Add(new PropertyMapping<InvoiceModel, Invoice>(_invoiceMapping));
			_propertyMappings.Add(new PropertyMapping<MaterialModel, Material>(_invoiceMapping));
		}
		public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
		{
			var propertyMapping = GetPropertyMapping<TSource, TDestination>();

			if (string.IsNullOrWhiteSpace(fields))
			{
				return true;
			}

			// the string is separated by ",", so we split it.
			var fieldsAfterSplit = fields.Split(',');

			// run through the fields clauses
			foreach (var field in fieldsAfterSplit)
			{
				// trim
				var trimmedField = field.Trim();

				// remove everything after the first " " - if the fields 
				// are coming from an orderBy string, this part must be 
				// ignored
				var indexOfFirstSpace = trimmedField.IndexOf(" ");
				var propertyName = indexOfFirstSpace == -1 ?
					trimmedField : trimmedField.Remove(indexOfFirstSpace);

				// find the matching property
				if (!propertyMapping.ContainsKey(propertyName))
				{
					return false;
				}
			}
			return true;
		}
		public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
		{
			//get matching mapping
			var matchingMapping = _propertyMappings
				.OfType<PropertyMapping<TSource, TDestination>>();

			if (matchingMapping.Count() == 1)
			{
				return matchingMapping.First()._mappingDictionary;
			}

			throw new Exception($"Cannot find exact property mapping instance " +
				$"for <{typeof(TSource)},{typeof(TDestination)}>");
		}
	}
	public class PropertyMapping<TSource, TDestination> : IPropertyMapping
	{
		public Dictionary<string, PropertyMappingValue> _mappingDictionary { get; private set; }

		public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
		{
			_mappingDictionary = mappingDictionary ?? throw new ArgumentNullException(nameof(mappingDictionary));
		}
	}
	public class PropertyMappingValue : IPropertyMappingValue
	{
		public IEnumerable<string> DestinationProperties { get; private set; }
		public bool Revert { get; private set; }

		public PropertyMappingValue(IEnumerable<string> destinationProperties, bool revert = false)
		{
			DestinationProperties = destinationProperties
				?? throw new ArgumentNullException(nameof(destinationProperties));
			Revert = revert;
		}
	}
}
