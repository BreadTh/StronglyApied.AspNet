using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using BreadTh.StronglyApied.AspNet.Core;

namespace BreadTh.StronglyApied.AspNet
{
    public class StronglyApiedInputFormatter : InputFormatter
    {
        public StronglyApiedInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("*/*"));
        }

        public override bool CanRead(InputFormatterContext context) => true;
        
        protected override bool CanReadType(Type type) => true;

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            using var bodyReader = new StreamReader(context.HttpContext.Request.Body, leaveOpen: true);
            
            var (result, errors) = 
                new ModelValidator()
                    .Parse(await bodyReader.ReadToEndAsync(), context.ModelType);
            
            if(errors.Count == 0)
                return await InputFormatterResult.SuccessAsync(result);            
            
            throw new BodyParseException(errors);
        }
    }
}
