<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="mx-auto mt-10" max-width="540">
          <v-card-title class="text-h2">Questionnaire</v-card-title>
          <v-card-subtitle class="text-h6">{{
            respondentName
          }}</v-card-subtitle>
          <v-card-text>
            <v-card
              v-for="(question, questionIndex) in questions"
              :key="questionIndex"
              class="mb-4"
            >
              <v-card-title class="text-subtitle-1">{{
                question.text
              }}</v-card-title>
              <v-list>
                <v-list-item
                  v-for="(answer, answerIndex) in question.answers"
                  :key="answerIndex"
                >
                  <v-list-item-action>
                    <v-checkbox
                      :value="isSelected(questionIndex, answer)"
                      @change="updateSelectedAnswers({questionIndex, answer})"
                    ></v-checkbox>
                  </v-list-item-action>
                  <v-list-item-content>
                    {{ answer.text }}
                  </v-list-item-content>
                </v-list-item>
              </v-list>
              <v-card-actions>
                <v-btn color="secondary" @click="submitQuestion(questionIndex)"
                  >Submit</v-btn
                >
              </v-card-actions>
            </v-card>
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary" @click="finishQuestionnaire">Finish</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapState, mapMutations, mapActions } from "vuex";

export default {
  data() {
    return {};
  },
  computed: {
    ...mapState("questionnaire", [
      "questions",
      "responses",
      "respondentAnswers",
      "respondentId",
      "respondentName",
    ]),
  },
  methods: {
    ...mapMutations("questionnaire", [
      "setQuestions",
      "setResponses",
      "setRespondentAnswers",
      "setRespondentId",
      "resetState",
    ]),
    ...mapActions("questionnaire", [
      "loadRespondents",
      "loadRespondentAnswers",
      "loadQuestionnaire",
      "updateSelectedAnswers",
      "submitQuestionAnswers",
    ]),
    isSelected(questionIndex, answer) {
      return this.responses[questionIndex].selectedAnswers.includes(answer);
    },

    async submitQuestion(questionIndex) {
      const selectedAnswers = this.responses[questionIndex].selectedAnswers;

      const questionId = this.questions[questionIndex].id;
      const selectedAnswerIds = selectedAnswers.map((answer) => answer.id);

      this.submitQuestionAnswers({ questionId, selectedAnswerIds });
    },
    finishQuestionnaire() {
      // Implement your logic for finishing the questionnaire
      // You can use this.responses to access all selected answers
    },
  },
  watch: {
    $route(to, from) {
      debugger;
      this.resetState();
      this.setRespondentId(to.params.respondentId);
      this.loadRespondents();
      this.loadQuestionnaire();
      this.loadRespondentAnswers();
    },
  },
  mounted() {
    this.loadRespondents();
    this.loadQuestionnaire();
    this.loadRespondentAnswers();
  },
};
</script>
