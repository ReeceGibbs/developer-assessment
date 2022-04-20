using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.ModelBinders
{
    public class TodoItemModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            if (bindingContext.HttpContext == null)
                throw new ArgumentNullException(nameof(bindingContext.HttpContext));

            var request = bindingContext.HttpContext.Request;

            try
            {
                Guid id = Guid.NewGuid();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                request.Body.ReadAsync(buffer, 0, buffer.Length);
                string rawBody = Encoding.UTF8.GetString(buffer);
                dynamic rawTodoItem;

                try { rawTodoItem = JObject.Parse(rawBody); }
                catch { throw new Exception("Request payload invalid format"); }

                if (!rawTodoItem.ContainsKey("description") || String.IsNullOrEmpty(rawTodoItem.description.Value.Trim()))
                    throw new Exception("Description is required");

                if (rawTodoItem.ContainsKey("id") && !Guid.TryParse(rawTodoItem.id.Value, out id))
                    throw new Exception("Id must be a valid guid");

                bindingContext.Result = ModelBindingResult.Success(new TodoItem
                {
                    Id = id,
                    Description = rawTodoItem.description.Value,
                    IsCompleted = rawTodoItem.ContainsKey("isCompleted") ? rawTodoItem.isCompleted.Value : false
                });
            }
            catch (Exception ex)
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.OriginalModelName, ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
