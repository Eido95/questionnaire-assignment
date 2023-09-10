<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="mx-auto" max-width="540">
          <v-card-title class="text-h2">Questionnaire</v-card-title>
          <v-card-subtitle class="text-h6">Eido</v-card-subtitle>
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
                      v-model="responses[questionIndex].selectedAnswers"
                      :value="answer"
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
import axios from "axios";

export default {
  data() {
    return {
      respondentId: 1,
      questions: [],
      responses: [],
      respondentAnswers: [],
    };
  },
  methods: {
    async loadRespondentAnswers() {
      axios
        .get(
          `http://localhost:5133/api/v1/questionnaire/RespondentAnswer?respondentId=${this.respondentId}`
        )
        .then((response) => {
          this.respondentAnswers = response.data;

          // Loop through respondentAnswers
          this.respondentAnswers.forEach((answer) => {
            // Find the corresponding question
            const questionIndex = this.questions.findIndex(
              (q) => q.id === answer.questionId
            );
            if (questionIndex !== -1) {
              // Find the corresponding answer
              const answerIndex = this.questions[
                questionIndex
              ].answers.findIndex((a) => a.id === answer.answerId);
              if (answerIndex !== -1) {
                // Check the corresponding checkbox
                this.responses[questionIndex].selectedAnswers.push(
                  this.questions[questionIndex].answers[answerIndex]
                );
              }
            }
          });
        })
        .catch((error) => {
          console.error("Error fetching respondent answers:", error);
        });
    },
    async loadQuestionnaire() {
      try {
        const response = await axios.get(
          "http://localhost:5133/api/v1/questionnaire/Questionnaire"
        );
        const questionnaireData = response.data[0];

        this.questions = questionnaireData.questions.map((question) => ({
          id: question.id,
          text: question.text,
          answers: question.answers.map((answer) => ({
            id: answer.id,
            text: answer.text,
          })),
        }));

        this.responses = Array.from({ length: this.questions.length }, () => ({
          selectedAnswers: [],
        }));
      } catch (error) {
        console.error(error);
      }
    },
    async submitQuestionAnswers(questionId, selectedAnswerIds) {
      const requestBody = {
        id: questionId,
        answerIds: selectedAnswerIds,
      };

      try {
        const response = await axios.put(
          `http://localhost:5133/api/v1/questionnaire/Question?respondentId=${this.respondentId}`,
          requestBody
        );
        console.log("Question answers submitted successfully:", response.data);
        console.log(requestBody);
      } catch (error) {
        console.error("Error submitting question answers:", error);
      }
    },

    async submitQuestion(questionIndex) {
      const selectedAnswers = this.responses[questionIndex].selectedAnswers;

      if (selectedAnswers.length > 0) {
        const questionId = this.questions[questionIndex].id;
        const selectedAnswerIds = selectedAnswers.map((answer) => answer.id);

        await this.submitQuestionAnswers(questionId, selectedAnswerIds);
      }
    },
    finishQuestionnaire() {
      // Implement your logic for finishing the questionnaire
      // You can use this.responses to access all selected answers
    },
  },
  mounted() {
    this.loadQuestionnaire();
    this.loadRespondentAnswers();
  },
};
</script>