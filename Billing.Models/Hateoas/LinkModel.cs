﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Billing.Models.Hateoas
{
	public class LinkModel
	{
        public string Href { get; private set; }
        public string Rel { get; private set; }
        public string Method { get; private set; }

        public LinkModel(string href, string rel, string method)
        {
            Href = href;
            Rel = rel;
            Method = method;
        }

    }
}
