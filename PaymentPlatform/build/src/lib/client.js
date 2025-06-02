/**
 * @typedef {object} Item
 * @property {string} currency - The currency code (e.g., "USD").
 * @property {string} description - A description of the item.
 * @property {string} id - A UUID (Universally Unique Identifier) representing the item's ID.
 * @property {boolean} isActive - Indicates whether the item is currently active.
 * @property {string} name - The name of the item.
 * @property {string} url - A URL associated with the item.
 */

/**
 * @typedef {Item[]} ItemArray
 * @description An array of Item objects.
 */

const client = {
    /**
     * @returns {ItemArray}
     */
    getProviders: async () => {
        return await fetch("/api/providers").then(async (response) => {
            return await response.json()
        }).catch((error) => {
            alert('Fetch error: ' + error);
            return [];
        });
    },

    /**
     * @param {Item} provider
     */
    addProvider: async (provider) => {
        return await fetch(`/api/providers/`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(provider),
        }).then((response) => {
            if (!response.ok) {
                console.error(response);
                alert('Response error:' + response.status);
            }

            return response.status;
        }).catch((error) => {
            console.log(error);

            alert('Fetch error: ' + error);
            return -1;
        });
    },

    /**
     * @param {Item} id
     */
    updateProvider: async (provider) => {
        return await fetch(`/api/providers/${provider.id}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(provider),
        }).then((response) => {
            if (!response.ok) {
                alert('Response error:' + response.status);
            }

            return response.status;
        }).catch((error) => {
            console.log(error);

            alert('Fetch error: ' + error);
            return -1;
        });
    },

    /**
     * @param {string} id
     */
    deleteProvider: async (id) => {
        return await fetch(`/api/providers/${id}`, {
            method: "DELETE",
        }).then((response) => {
            if (!response.ok) {
                alert('Response error:' + response.status);
            }

            return response.status;
        }).catch((error) => {
            console.log(error);

            alert('Fetch error: ' + error);
            return -1;
        });
    },

    getProvider: async (id) => {
        return await fetch(`/api/providers/${id}`).then(async (response) => {
            if (!response.ok) {
                alert('Response error:' + response.status);
            }

            return await response.json()
        }).catch((error) => {
            console.log(error);

            alert('Fetch error: ' + error);
            return [];
        });
    }

}

export default client;