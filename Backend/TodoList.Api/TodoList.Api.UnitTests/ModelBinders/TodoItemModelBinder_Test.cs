using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TodoList.Api.ModelBinders;
using TodoList.Api.Models;
using Xunit;

namespace TodoList.Api.UnitTests.ModelBinders
{
    public class TodoItemModelBinder_Test
    {
        private readonly TodoItemModelBinder _todoItemModelBinder;
        private readonly Mock<ModelBindingContext> _modelBindingContext;
        private readonly Mock<HttpContext> _httpContext;
        private readonly Mock<HttpRequest> _httpRequest;

        public TodoItemModelBinder_Test()
        {
            _httpRequest = new Mock<HttpRequest>();
            _httpContext = new Mock<HttpContext>();
            _modelBindingContext = new Mock<ModelBindingContext>();
            _todoItemModelBinder = new TodoItemModelBinder();
        }

        [Fact]
        public async Task TodoItemModelBinder_retains_guid_if_passed()
        {
            var mockTodoItem = "{\"id\":\"367d2426-4a01-4c0e-8a7a-0c4a7b1d7205\",\"description\":\"TestDesc\",\"isCompleted\":false}";
            _httpRequest.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(mockTodoItem)));
            _httpRequest.Setup(r => r.ContentLength).Returns(mockTodoItem.Length);

            _httpContext.Setup(c => c.Request).Returns(_httpRequest.Object);

            _modelBindingContext.Setup(c => c.HttpContext).Returns(_httpContext.Object);
            _modelBindingContext.Setup(c => c.ModelState).Returns(new ModelStateDictionary());
            _modelBindingContext.SetupProperty(c => c.Result);

            await _todoItemModelBinder.BindModelAsync(_modelBindingContext.Object);

            Assert.Equal(0, string.Compare(mockTodoItem, JsonConvert.SerializeObject(_modelBindingContext.Object.Result.Model), true));
        }

        [Fact]
        public async Task TodoItemModelBinder_creates_guid_if_not_passed()
        {
            var mockTodoItem = "{\"description\":\"TestDesc\",\"isCompleted\":false}";
            _httpRequest.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(mockTodoItem)));
            _httpRequest.Setup(r => r.ContentLength).Returns(mockTodoItem.Length);

            _httpContext.Setup(c => c.Request).Returns(_httpRequest.Object);

            _modelBindingContext.Setup(c => c.HttpContext).Returns(_httpContext.Object);
            _modelBindingContext.Setup(c => c.ModelState).Returns(new ModelStateDictionary());
            _modelBindingContext.SetupProperty(c => c.Result);

            await _todoItemModelBinder.BindModelAsync(_modelBindingContext.Object);

            Assert.IsType<Guid>(((TodoItem)_modelBindingContext.Object.Result.Model).Id);
        }

        [Fact]
        public async Task TodoItemModelBinder_sets_completed_to_false_if_not_passed()
        {
            var mockTodoItem = "{\"description\":\"TestDesc\"}";
            _httpRequest.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(mockTodoItem)));
            _httpRequest.Setup(r => r.ContentLength).Returns(mockTodoItem.Length);

            _httpContext.Setup(c => c.Request).Returns(_httpRequest.Object);

            _modelBindingContext.Setup(c => c.HttpContext).Returns(_httpContext.Object);
            _modelBindingContext.Setup(c => c.ModelState).Returns(new ModelStateDictionary());
            _modelBindingContext.SetupProperty(c => c.Result);

            await _todoItemModelBinder.BindModelAsync(_modelBindingContext.Object);

            Assert.False(((TodoItem)_modelBindingContext.Object.Result.Model).IsCompleted);
        }

        [Fact]
        public async Task TodoItemModelBinder_does_not_accept_invalid_json()
        {
            var mockTodoItem = "{\"description\":\"TestDesc\",\"isCompleted\":false";
            _httpRequest.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(mockTodoItem)));
            _httpRequest.Setup(r => r.ContentLength).Returns(mockTodoItem.Length);

            _httpContext.Setup(c => c.Request).Returns(_httpRequest.Object);

            _modelBindingContext.Setup(c => c.HttpContext).Returns(_httpContext.Object);
            _modelBindingContext.Setup(c => c.ModelState).Returns(new ModelStateDictionary());

            await _todoItemModelBinder.BindModelAsync(_modelBindingContext.Object);

            Assert.Equal("Request payload invalid format", _modelBindingContext.Object.ModelState.Values.ElementAt(0).Errors.ElementAt(0).ErrorMessage);
        }

        [Fact]
        public async Task TodoItemModelBinder_no_description_failure()
        {
            var mockTodoItem = "{\"description\":\"\",\"isCompleted\":false}";
            _httpRequest.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(mockTodoItem)));
            _httpRequest.Setup(r => r.ContentLength).Returns(mockTodoItem.Length);

            _httpContext.Setup(c => c.Request).Returns(_httpRequest.Object);

            _modelBindingContext.Setup(c => c.HttpContext).Returns(_httpContext.Object);
            _modelBindingContext.Setup(c => c.ModelState).Returns(new ModelStateDictionary());

            await _todoItemModelBinder.BindModelAsync(_modelBindingContext.Object);

            Assert.Equal("Description is required", _modelBindingContext.Object.ModelState.Values.ElementAt(0).Errors.ElementAt(0).ErrorMessage);
        }

        [Fact]
        public async Task TodoItemModelBinder_invalid_guid_failure()
        {
            var mockTodoItem = "{\"id\":\"035a454a-b9f9-48b5-b3fe-c1324\",\"description\":\"TestDesc\",\"isCompleted\":false}";
            _httpRequest.Setup(r => r.Body).Returns(new MemoryStream(Encoding.UTF8.GetBytes(mockTodoItem)));
            _httpRequest.Setup(r => r.ContentLength).Returns(mockTodoItem.Length);

            _httpContext.Setup(c => c.Request).Returns(_httpRequest.Object);

            _modelBindingContext.Setup(c => c.HttpContext).Returns(_httpContext.Object);
            _modelBindingContext.Setup(c => c.ModelState).Returns(new ModelStateDictionary());

            await _todoItemModelBinder.BindModelAsync(_modelBindingContext.Object);

            Assert.Equal("Id must be a valid guid", _modelBindingContext.Object.ModelState.Values.ElementAt(0).Errors.ElementAt(0).ErrorMessage);
        }
    }
}
