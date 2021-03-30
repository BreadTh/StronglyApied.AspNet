using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

using BreadTh.StronglyApied.AspNet.Core;
using BreadTh.StronglyApied.Attributes;

namespace BreadTh.StronglyApied.AspNet
{
    public class StronglyApiedInputFormatter : InputFormatter
    {
        public StronglyApiedInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("*/*"));
        }

        public override bool CanRead(InputFormatterContext context) => true;
        
        protected override bool CanReadType(Type type) =>
            type.GetCustomAttribute<StronglyApiedRootAttribute>(false) != null;

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            using var bodyReader = new StreamReader(context.HttpContext.Request.Body);
            
            var (result, errors) = 
                new ModelValidator()
                    .Parse(await bodyReader.ReadToEndAsync(), context.ModelType);
            
            if(errors.Count == 0)
                return await InputFormatterResult.SuccessAsync(result);            
            
            throw new BodyParseException(errors);
        }
    }
}
