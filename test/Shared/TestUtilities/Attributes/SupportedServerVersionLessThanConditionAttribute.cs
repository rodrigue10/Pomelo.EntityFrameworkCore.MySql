﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestUtilities.Xunit;

namespace Pomelo.EntityFrameworkCore.MySql.Tests.TestUtilities.Attributes
{
    // For facts and theories, they must be defined as conditional (ConditionalFact, ConditionalTheory) for this attribute to work.
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SupportedServerVersionLessThanConditionAttribute : Attribute, ITestCondition
    {
        protected string[] PropertiesOrVersions { get; }

        public SupportedServerVersionLessThanConditionAttribute(params string[] propertiesOrVersions)
        {
            PropertiesOrVersions = propertiesOrVersions;
        }

        public virtual ValueTask<bool> IsMetAsync()
        {
            var currentVersion = AppConfig.ServerVersion;
            var isMet = !PropertiesOrVersions.Any(s => currentVersion.Supports.PropertyOrVersion(s));

            if (!isMet && string.IsNullOrEmpty(Skip))
            {
                Skip = $"The test is not supported on server version {currentVersion}.";
            }

            return new ValueTask<bool>(isMet);
        }

        public virtual string SkipReason => Skip;
        public virtual string Skip { get; set; }
    }
}
