using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Service.Services;

namespace TodoList.UnitTests.Api.Controllers
{
    public class TodoItemsControllerTests_Base
    {
        protected readonly Mock<ITodoItemsService> _todoItemsService;
        protected readonly Mock<IMapper> _mapperMock;
        protected readonly TodoItemsController _todoItemsController;

        // The test suites that inherit from this will only include happy path tests as the actual functionality of the exception filter and service logic will be tested separately
        public TodoItemsControllerTests_Base()
        {
            _todoItemsService = new Mock<ITodoItemsService>();
            _mapperMock = new Mock<IMapper>();
            _todoItemsController = new TodoItemsController(_todoItemsService.Object, _mapperMock.Object);
        }
    }
}
