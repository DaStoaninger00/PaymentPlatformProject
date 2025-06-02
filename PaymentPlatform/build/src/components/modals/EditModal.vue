<script setup>
import { ref } from "vue";
import client from "../../lib/client";

const props = defineProps({
    uuid: {
        type: String,
        required: true,
    },
});
const emit = defineEmits(['updateList'])

const modal = ref(null);

const needsFetch = ref(true);

const name = ref("");
const description = ref("");
const url = ref("");
const currency = ref("");
const isActive = ref(true);

const toggleModal = async () => {
    modal.value.toggleAttribute("open");

    if (needsFetch) {
        const provider = await client.getProvider(props.uuid);

        name.value = provider.name;
        description.value = provider.description;
        url.value = provider.url;
        currency.value = provider.currency;
        isActive.value = provider.isActive;

        needsFetch.value = false;
    }
};

const saveProvider = async () => {
    const provider = {
        id: props.uuid,
        name: name.value,
        description: description.value,
        url: url.value,
        currency: currency.value,
        isActive: isActive.value,
    };

    await client.updateProvider(provider);

    needsFetch.value = false;
    emit('updateList');
};
</script>

<template>
    <div>
        <button class="btn btn-outline btn-info w-full" @click="toggleModal">Edit</button>

        <dialog id="edit_modal" class="modal" ref="modal">
            <div class="modal-box w-fit min-w-[450px]">
                <h3 class="text-lg font-bold">Editing</h3>

                <div class="modal-body" v-if="needsFetch">
                    loading...
                </div>
                <div class="modal-body" v-else>
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">Name</legend>
                        <input type="text" class="input w-full" v-model="name" />
                    </fieldset>
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">Description</legend>
                        <textarea type="text" class="textarea w-full" v-model="description"></textarea>
                    </fieldset>
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">URL</legend>
                        <input type="text" class="input w-full" v-model="url" />
                    </fieldset>
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">Currency</legend>
                        <input type="text" class="input w-full" v-model="currency" />
                    </fieldset>
                    <fieldset class="fieldset">
                        <legend class="fieldset-legend">Is Active</legend>
                        <input type="checkbox" class="checkbox" v-model="isActive" />
                    </fieldset>
                </div>

                <div class="modal-action">
                    <form method="dialog">
                        <button class="btn btn-sm">Close</button>
                        <button class="btn btn-sm btn-success" @click="saveProvider">Save</button>
                    </form>
                </div>
            </div>
        </dialog>
    </div>
</template>
