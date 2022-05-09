const apiUrl = process.env.REACT_APP_API_URL;
const apiKey = process.env.REACT_APP_API_KEY;

export const getTodoItems = async () => {
    const response = await fetch(apiUrl + "/api/todoitems", {
        method: "GET",
        headers: {
            "API_KEY": apiKey
        }
    });

    const result = await response.json();

    return result.value ?? result;
};  