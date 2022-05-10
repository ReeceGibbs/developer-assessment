import fetchMock from "jest-fetch-mock";
import { createTodoItem, getTodoItems, updateTodoItem } from './todoItems.api';

fetchMock.enableMocks();

describe("[api] TodoItems", () => {
    beforeEach(() => {
        fetch.resetMocks();
    })

    it("get todoItems success", async () => {
        const mockTodoItem = JSON.stringify({
            data: {
                id: "1234",
                description: "Test 0",
                isCompleted: false
            },
            success: true,
            error: null
        });

        fetch.mockResponseOnce(mockTodoItem);

        const response = await getTodoItems();

        expect(JSON.stringify(response))
            .toEqual(mockTodoItem);
    });

    it("get todoItems service unavaliable failure", async () => {
        const expectedResponse = JSON.stringify({
            error: {
                key: "ServiceUnavailable",
                value: "Server currently unavailable"
            }
        });

        fetch.mockImplementation(() => {
            throw new Error();
        });

        const response = await getTodoItems();

        expect(JSON.stringify(response)).toEqual(expectedResponse);
    });

    it("create todoItem success", async () => {
        const mockCallBack = jest.fn((response) => response);

        const mockTodoItem = {
            data: {
                id: "1234",
                description: "Test 0",
                isCompleted: false
            },
            success: true,
            error: null
        };

        fetch.mockResponseOnce(JSON.stringify(mockTodoItem));

        await createTodoItem(mockTodoItem, mockCallBack);

        expect(mockCallBack.mock.calls.length).toBe(1);

        expect(JSON.stringify(mockCallBack.mock.results[0].value))
            .toEqual(JSON.stringify(mockTodoItem));
    });

    it("create todoItem server-side failure", async () => {
        const mockCallBack = jest.fn((response) => response);

        const mockResponse = {
            value: {
                data: null,
                success: false,
                error: {
                    key: "Conflict",
                    value: "Description already exists"
                }
            }
        };

        fetch.mockResponseOnce(JSON.stringify(mockResponse));

        await createTodoItem({}, mockCallBack);

        expect(mockCallBack.mock.calls.length).toBe(1);

        expect(JSON.stringify(mockCallBack.mock.results[0].value))
            .toEqual(JSON.stringify(mockResponse.value));
    });

    it("create todoItem service unavaliable failure", async () => {
        const mockCallBack = jest.fn((response) => response);

        const expectedResponse = JSON.stringify({
            error: {
                key: "ServiceUnavailable",
                value: "Server currently unavailable"
            }
        });

        fetch.mockImplementation(() => {
            throw new Error();
        });

        await createTodoItem({}, mockCallBack);

        expect(mockCallBack.mock.calls.length).toBe(1);

        expect(JSON.stringify(mockCallBack.mock.results[0].value))
            .toEqual(expectedResponse);
    });

    it("update todoItem success", async () => {
        const mockCallBack = jest.fn((response) => response);

        const mockTodoItem = {
            data: {
                id: "1234",
                description: "Test 0",
                isCompleted: false
            },
            success: true,
            error: null
        };

        fetch.mockResponseOnce(JSON.stringify(mockTodoItem));

        await updateTodoItem(mockTodoItem, mockCallBack);

        expect(mockCallBack.mock.calls.length).toBe(1);

        expect(JSON.stringify(mockCallBack.mock.results[0].value))
            .toEqual(JSON.stringify(mockTodoItem));
    });

    it("update todoItem server-side failure", async () => {
        const mockCallBack = jest.fn((response) => response);

        const mockResponse = {
            value: {
                data: null,
                success: false,
                error: {
                    key: "Conflict",
                    value: "Description already exists"
                }
            }
        };

        fetch.mockResponseOnce(JSON.stringify(mockResponse));

        await updateTodoItem({}, mockCallBack);

        expect(mockCallBack.mock.calls.length).toBe(1);

        expect(JSON.stringify(mockCallBack.mock.results[0].value))
            .toEqual(JSON.stringify(mockResponse.value));
    });

    it("update todoItem service unavaliable failure", async () => {
        const mockCallBack = jest.fn((response) => response);

        const expectedResponse = JSON.stringify({
            error: {
                key: "ServiceUnavailable",
                value: "Server currently unavailable"
            }
        });

        fetch.mockImplementation(() => {
            throw new Error();
        });

        await updateTodoItem({}, mockCallBack);

        expect(mockCallBack.mock.calls.length).toBe(1);

        expect(JSON.stringify(mockCallBack.mock.results[0].value))
            .toEqual(expectedResponse);
    });
});