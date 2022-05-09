import './TodoListItem.css'
import { Col, Row, Form } from 'react-bootstrap';
import React, { useState } from 'react'
import PropTypes from "prop-types";
import colours from '../constants/colours';
import { PlusSquare } from 'react-bootstrap-icons';

const TodoListItem = (props) => {
    const [description, setDescription] = useState("");

    const isInputInvalid = description.length === 0 || description.length > 20;

    const handleTextChange = (event) => {
        setDescription(event.target.value);
    };

    const handleAdd = () => {
        if (!isInputInvalid) {
            props.handleAdd(description);
            setDescription("");
        }
    }

    return (
        <Row className={colours[props.colour]}>
            <Col>
                {props.id === "new_item" ?
                    <Form.Control
                        className="textBox"
                        type="text"
                        onChange={handleTextChange}
                        isInvalid={isInputInvalid}
                        isValid={!isInputInvalid}
                        value={description}
                    /> :
                    <h5 className={props.isCompleted ? "isCompleted" : undefined}>{props.description}</h5>}
            </Col>
            <Col className="clearfix">
                {props.id === "new_item" ?
                    <PlusSquare
                        className="addButton floatRight"
                        onClick={handleAdd}
                    /> :
                    <Form.Check
                        className="floatRight"
                        type="checkbox"
                        id={props.id}
                        defaultChecked={props.isCompleted}
                        onChange={props.handleChecked}
                    />}
            </Col>
        </Row>
    );
}

TodoListItem.propTypes = {
    id: PropTypes.string,
    description: PropTypes.string,
    isCompleted: PropTypes.bool,
    colour: PropTypes.number,
    handleChecked: PropTypes.func,
    handleAdd: PropTypes.func
}

export default TodoListItem