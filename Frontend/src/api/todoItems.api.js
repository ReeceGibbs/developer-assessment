const apiUrl = process.env.REACT_APP_API_URL;
const apiKey = process.env.REACT_APP_API_KEY;

export const getTodoItems = async () => {
    try {
        const response = await fetch(apiUrl + "/api/todoitems", {
            method: "GET",
            headers: {
                "API_KEY": apiKey
            }
        });

        const result = await response.json();

        return result.value ?? result;
    }
    catch {
        return ({
            error: {
                key: "ServiceUnavailable",
                value: "Server currently unavailable"
            }
        });
    }
};

export const createTodoItem = async (payload, finalize) => {
    try {
        const response = await fetch(apiUrl + "/api/todoitems", {
            method: "POST",
            headers: {
                "API_KEY": apiKey
            },
            body: JSON.stringify(payload)
        });

        const result = await response.json();

        finalize(result.value ?? result);
    }
    catch {
        finalize({
            error: {
                key: "ServiceUnavailable",
                value: "Server currently unavailable"
            }
        });
    }
};

export const updateTodoItem = async (payload, finalize) => {
    try {
        const response = await fetch(apiUrl + "/api/todoitems", {
            method: "PUT",
            headers: {
                "API_KEY": apiKey
            },
            body: JSON.stringify(payload)
        });

        const result = await response.json();

        finalize(result.value ?? result);
    }
    catch {
        finalize({
            error: {
                key: "ServiceUnavailable",
                value: "Server currently unavailable"
            }
        });
    }
};