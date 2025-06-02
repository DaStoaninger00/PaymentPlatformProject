<script setup>
import { ref, onMounted, computed } from "vue";
import client from "../lib/client.js";

import NewModal from "./modals/NewModal.vue";
import EditModal from "./modals/EditModal.vue";
import DeleteModal from "./modals/DeleteModal.vue";
import TestModal from "./modals/TestModal.vue";

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

/**
 * @type {ItemArray}
 */
const provider = ref([]);

const search = ref("");

const updateProviders = async () => {
    client.getProviders().then((data) => {
        provider.value = data;
    });
};

const filteredProviders = computed(() => {
    return provider.value.filter((item) => {
        if (!search.value) {
            return true
        };

        const nameFound = item.name.toLowerCase().includes(search.value.toLowerCase());
        const idFound = item.id.toLowerCase().includes(search.value.toLowerCase());

        return nameFound || idFound;
    });
});

// Update providers on mount
onMounted(updateProviders);
</script>

<template>

    <div>
        <input type="text" class="input" v-model="search" placeholder="Search...">

        <div class="overflow-x-auto">
            <table class="table">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Name</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in filteredProviders" :key="item.id">
                        <td>{{ item.id }}</td>
                        <td>{{ item.name }}</td>
                        <td>
                            <div class="mb-2">
                                <DeleteModal :uuid="item.id" @updateList="updateProviders"></DeleteModal>
                            </div>
                            <div class="mb-2">
                                <EditModal :uuid="item.id" @updateList="updateProviders"></EditModal>
                            </div>
                            <div>
                                <TestModal :uuid="item.id"></TestModal>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <NewModal @updateList="updateProviders"></NewModal>
    </div>
</template>
