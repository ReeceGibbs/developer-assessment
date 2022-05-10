import TodoList from "./TodoList";
import { fireEvent, screen } from "@testing-library/react";
import { Wrapper } from "react-test-wrapper/react-testing-library";

const component = new Wrapper(TodoList);

describe("[components] <TodoList />", () => {
    it("empty todo list renders correctly", () => {
        component.render();

        expect(
            screen.getByTestId(
                "textBox"
            )
        ).toBeInTheDocument();

        expect(
            screen.getByTestId(
                "addButton"
            )
        ).toBeInTheDocument();
    });

    it("empty description is invalid", () => {
        component.render();

        expect(
            screen.getByTestId(
                "textBox"
            ).classList.contains("is-invalid")
        ).toBe(true);
    });

    it("description less than 20 chars is valid", () => {
        component.render();

        const input = screen.getByTestId(
            "textBox"
        );

        fireEvent.change(input, { target: { value: "less 20 chars" } });

        expect(input.classList.contains("is-invalid")).toBe(false);
    });

    it("description over 20 chars is valid", () => {
        component.render();

        const input = screen.getByTestId(
            "textBox"
        );

        fireEvent.change(input, { target: { value: "this is a description that exceeds 20 characters" } });

        expect(input.classList.contains("is-invalid")).toBe(true);
    });
});