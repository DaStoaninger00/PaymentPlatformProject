<script setup>
import { ref } from "vue";
import client from "../../lib/client";

const modal = ref(null);

const toggleModal = () => {
    modal.value.toggleAttribute("open");
};
const emit = defineEmits(['updateList'])

const name = ref("");
const description = ref("");
const url = ref("");
const currency = ref("");
const isActive = ref(false);

const createProvider = async () => {
    const provider = {
        id: crypto.randomUUID(),
        name: name.value ?? "",
        url: url.value ?? "",
        currency: currency.value ?? "",
        description: description.value ?? "",
        isActive: isActive.value,
    };

    await client.addProvider(provider)
    emit('updateList');
}

</script>

<template>
    <div>
        <button class="btn btn-outline btn-accent w-full" @click="toggleModal">Create Provider</button>

        <dialog id="new_modal" class="modal" ref="modal">
            <div class="modal-box w-fit min-w-[450px]">
                <h3 class="text-lg font-bold">New Provider</h3>

                <div class="modal-body">
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">Name</legend>
                        <input type="text" class="input w-full" v-model="name" required />
                    </fieldset>
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">Description</legend>
                        <textarea type="text" class="textarea w-full" v-model="description"></textarea>
                    </fieldset>
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">URL</legend>
                        <input type="text" class="input w-full" v-model="url" required />
                    </fieldset>
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">Currency</legend>
                        <input type="text" class="input w-full" v-model="currency" required />
                    </fieldset>
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">Is Active</legend>
                        <input type="checkbox" class="checkbox" v-model="isActive" />
                    </fieldset>
                </div>

                <div class="modal-action">
                    <form method="dialog">
                        <button class="btn btn-sm">Close</button>
                        <button class="btn btn-sm btn-success" @click="createProvider">Create</button>
                    </form>
                </div>
            </div>
        </dialog>
    </div>
</template>
