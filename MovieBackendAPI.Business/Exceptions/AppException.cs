using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace MovieBackendAPI.Business.Exceptions
{
    public class AppException : Exception
    {
        public string StatusCode { get; }
        public string StatusMessage { get; set; }
        [Display(Name = "Message")]
        public string MainMessage { get; set; }
        public IDictionary<string, string[]> ValidationErrors { get; }


        public string FormattedError { get; }


        protected AppException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
        public AppException(string message) : base(message)
        {
            StatusCode = Domain.Const.ResponseCode.GENERIC_EXCEPTION;
            StatusMessage = message;
            MainMessage = message;
        }


        public AppException(List<ValidationFailure> failures)
            : this("One or more validation failures have occurred.")
        {
            ValidationErrors = new Dictionary<string, string[]>();
            IEnumerable<string> propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (string propertyName in propertyNames)
            {
                string[] propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                FormattedError += string.Join(",", failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)) + ", ";

                ValidationErrors.Add(propertyName, propertyFailures);
            }
        }
    }
}