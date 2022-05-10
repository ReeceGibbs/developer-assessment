import TodoListItem from "./TodoListItem";
import { screen } from "@testing-library/react";
import { Wrapper } from "react-test-wrapper/react-testing-library";

const component = new Wrapper(TodoListItem);

describe("[components] <TodoListItem />", () => {
    it("todo list item description renders correctly", () => {
        component.withProps({
            id: "1234",
            description: "Test Item",
            isCompleted: false,
            colour: 0,
            handleChecked: () => { },
            handleAdd: () => { }
        }).render();

        expect(
            screen.getByText(
                "Test Item"
            )
        ).toBeInTheDocument();
    });

    it("todo list item false checkbox renders correctly", () => {
        component.withProps({
            id: "1234",
            description: "Test Item",
            isCompleted: false,
            colour: 0,
            handleChecked: () => { },
            handleAdd: () => { }
        }).render();

        expect(
            screen.getByTestId(
                "formCheck"
            )
        ).not.toBeChecked();
    });

    it("todo list item true checkbox renders correctly", () => {
        component.withProps({
            id: "1234",
            description: "Test Item",
            isCompleted: true,
            colour: 0,
            handleChecked: () => { },
            handleAdd: () => { }
        }).render();

        expect(
            screen.getByTestId(
                "formCheck"
            )
        ).toBeChecked();
    });
});