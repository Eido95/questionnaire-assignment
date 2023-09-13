<template>
  <questionnaire />
</template>

<script>
import Questionnaire from "@/components/Questionnaire.vue";

export default {
  name: "QuestionnaireView",

  props: ["respondentId"],

  components: {
    Questionnaire,
  },
};
</script>
