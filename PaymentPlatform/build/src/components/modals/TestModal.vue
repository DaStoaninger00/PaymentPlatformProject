<script setup>
import { ref } from "vue";
import client from "../../lib/client";

const props = defineProps({
    uuid: {
        type: String,
        required: true,
    },
});

const modal = ref(null);

const fetching = ref(true);

const status = ref(null);
const jsonResponse = ref("");
const time = ref(null);

const amount = ref(null);
const currency = ref("");
const description = ref("");
const referenceId = ref("");

const toggleModal = async () => {
    modal.value.toggleAttribute("open");
    amount.value = null;
    currency.value = null;
    description.value = null;
    referenceId.value = null;
};

const sendTest = async () => {
    fetching.value = true;

    status.value = null;
    jsonResponse.value = "";
    time.value = null;

    const provider = await client.getProvider(props.uuid);

    const url = provider.url;

    const testData = {
        amount: amount.value,
        currency: currency.value,
        description: description.value,
        referenceId: referenceId.value
    };

    await fetch("/api/simulate?providerId=" + props.uuid, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(testData),
    })
        .then(async (response) => {
            const json = await response.json();

            status.value = response.status;
            jsonResponse.value = JSON.stringify(json);
        })
        .catch((error) => {
            status.value = -1;
            jsonResponse.value = "Failed to fetch";
        })
        .finally(() => {
            fetching.value = false;
            time.value = new Date().toLocaleTimeString();
        });
};

</script>

<template>
    <div>
        <button class="btn btn-outline btn-accent w-full" @click="toggleModal">Test</button>

        <dialog id="edit_modal" class="modal" ref="modal">
            <div class="modal-box w-fit min-w-[30%]">
                <h3 class="text-lg font-bold">Test Values</h3>
                <div class="modal-body">
                    <div class="flex w-full flex-col">
                        <div>
                            <fieldset class="fieldset">
                                <legend class="fieldset-legend">Amount</legend>
                                <input type="text" class="input w-full" v-model="amount" />
                            </fieldset>
                            <fieldset class="fieldset">
                                <legend class="fieldset-legend">Currency</legend>
                                <input type="text" class="input w-full" v-model="currency" />
                            </fieldset>
                            <fieldset class="fieldset">
                                <legend class="fieldset-legend">Description</legend>
                                <input type="text" class="input w-full" v-model="description" />
                            </fieldset>
                            <fieldset class="fieldset">
                                <legend class="fieldset-legend">ReferenceID</legend>
                                <input type="text" class="input w-full" v-model="referenceId" />
                            </fieldset>
                        </div>
                        <br>
                        <p>
                            time: {{ time }}
                        </p>
                        <p>
                            status: {{ status }}
                        </p>
                        <div class="divider">Response</div>
                        <div class="card bg-base-300 rounded-box grid h-20 place-items-center p-5"
                            style="height: auto;">
                            <pre style="white-space: pre-wrap;">{{ jsonResponse }}</pre>
                        </div>
                    </div>
                </div>

                <div class="modal-action">
                    <form method="dialog">
                        <button type="button" class="btn btn-sm btn-success" @click="sendTest">Send Test</button>
                        <button class="btn btn-sm">Close</button>
                    </form>
                </div>
            </div>
        </dialog>
    </div>
</template>
