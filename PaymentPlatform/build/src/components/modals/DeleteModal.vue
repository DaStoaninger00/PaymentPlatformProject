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

const deleteProvider = async () => {
    await client.deleteProvider(props.uuid);

    emit('updateList');
};

const toggleModal = async () => {
    modal.value.toggleAttribute("open");
};
</script>

<template>
    <div>
        <button class="btn btn-outline btn-error w-full" @click="toggleModal">Delete</button>

        <dialog id="edit_modal" class="modal" ref="modal">
            <div class="modal-box w-fit min-w-[450px]">
                <h3 class="text-lg font-bold">Delete Provider</h3>

                <div class="modal-body">
                    id: {{ props.uuid }}?
                </div>

                <div class="modal-action">
                    <form method="dialog">
                        <button class="btn btn-sm">Close</button>
                        <button class="btn btn-sm btn-error" @click="deleteProvider">Delete</button>
                    </form>
                </div>
            </div>
        </dialog>
    </div>
</template>
