import './TodoList.css'
import { Container, Toast } from 'react-bootstrap';
import React, { useState, useEffect } from 'react'
import TodoListItem from './TodoListItem';
import { createTodoItem, getTodoItems, updateTodoItem } from '../api/todoItems.api';

const TodoList = () => {
    const [todoItems, setTodoItems] = useState([]);
    const [toastError, setToastError] = useState("");

    const handleChecked = (event) => {
        handleToastClose();

        let tempTodoItems = [...todoItems];
        const todoItemIndex = tempTodoItems.findIndex(todoItem => todoItem.id === event.target.id);

        if (todoItemIndex > -1) {
            const payload = {
                ...todoItems[todoItemIndex],
                isCompleted: event.target.checked
            };

            updateTodoItem(payload, (response) => {
                if (response.error) {
                    setToastError(response.error.value);
                    return;
                }

                tempTodoItems.splice(todoItemIndex, 1);
                tempTodoItems.push(response.data);

                setTodoItems(tempTodoItems);
            })
        }
    };

    const handleAdd = async (description) => {
        handleToastClose();

        const payload = {
            description: description
        };

        await createTodoItem(payload, (response) => {
            let tempTodoItems = [...todoItems]

            if (response.error) {
                setToastError(response.error.value);
                return;
            }

            tempTodoItems.push(response.data);
            setTodoItems(tempTodoItems);
        });
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

    const handleToastClose = () => {
        setToastError("");
    };

    useEffect(() => {
        const fetchTodoItems = async () => {
            const response = await getTodoItems();

            setTodoItems(response.data ?? []);

            if (response.error)
                setToastError(response.error.value);
        }

        fetchTodoItems();
    }, []);

    return (
        <div>
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
            <Toast
                className="toast"
                show={toastError.length > 0}
                onClose={handleToastClose}
            >
                <Toast.Header>
                    <strong className="me-auto red">Error</strong>
                </Toast.Header>
                <Toast.Body>{toastError}</Toast.Body>
            </Toast>
        </div>
    );
}

export default TodoList