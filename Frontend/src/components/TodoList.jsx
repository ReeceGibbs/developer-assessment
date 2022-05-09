import './TodoList.css'
import { Container } from 'react-bootstrap';
import React, { useState } from 'react'
import TodoListItem from './TodoListItem';

const mockTodoItems = [
    {
        id: "a7229afa-4295-4b5f-b123-5f9a132e2d01",
        description: "Item 1",
        isCompleted: false
    },
    {
        id: "3ec67717-319c-4f1d-84dc-19a1d32211fb",
        description: "Item 2",
        isCompleted: false
    },
    {
        id: "8f5103ba-eba5-480a-9d9f-0043c5e3e4f8",
        description: "Item 3",
        isCompleted: false
    },
    {
        id: "391d27b0-2605-47ce-ae66-f172cd210277",
        description: "Item 4",
        isCompleted: false
    },
    {
        id: "8156d624-a0ba-4211-b28b-7f7e66afb629",
        description: "Item 5",
        isCompleted: false
    },
    {
        id: "37265060-6125-47f5-bf5a-febcd7b89aae",
        description: "Item 6",
        isCompleted: true
    }
];

const TodoList = () => {
    const [todoItems, setTodoItems] = useState(mockTodoItems);

    const handleChecked = (event) => {
        let tempTodoItems = [...todoItems];
        const todoItemIndex = tempTodoItems.findIndex(todoItem => todoItem.id === event.target.id);

        if (todoItemIndex > -1) {
            const updatedTodoItem = {
                ...todoItems[todoItemIndex],
                isCompleted: event.target.checked
            };

            tempTodoItems.splice(todoItemIndex, 1);
            tempTodoItems.push(updatedTodoItem);
            
            setTodoItems(tempTodoItems);
        }
    };

    const handleAdd = (description) => {
        let tempTodoItems = [...todoItems]
        tempTodoItems.push({
            id: (new Date()).getUTCMilliseconds().toString(),
            description: description,
            isCompleted: false
        });

        setTodoItems(tempTodoItems);
    }

    let colourCounter = -1;

    const generateTodoItem = (todoItem) => {
        colourCounter++;

        if (colourCounter > 3)
            colourCounter = 0;

        return (
            <TodoListItem
                key={todoItem.id}
                id={todoItem.id}
                description={todoItem.description}
                isCompleted={todoItem.isCompleted}
                colour={colourCounter}
                handleChecked={handleChecked}
            />
        );
    };

    return (
        <Container>
            <h1 className="header">Todo</h1>
            {todoItems.length > 0 &&
                todoItems.map((todoItem) => !todoItem.isCompleted && generateTodoItem(todoItem))}
            <TodoListItem
                id="new_item"
                isCompleted={false}
                colour={4}
                handleAdd={handleAdd}
            />
            <br />
            <h1 className="header">Completed</h1>
            {todoItems.length > 0 &&
                todoItems.map((todoItem) => todoItem.isCompleted && generateTodoItem(todoItem))}
        </Container>
    );
}

export default TodoList