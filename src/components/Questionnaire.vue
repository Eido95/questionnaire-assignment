<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-card class="mx-auto" max-width="540">
          <v-card-title>Questionnaire</v-card-title>
          <v-card-text>
            <v-card
              v-for="(question, index) in questions"
              :key="index"
              class="mb-4"
            >
              <v-card-title class="text-subtitle-1">{{
                question.text
              }}</v-card-title>
              <v-list>
                <v-list-item
                  v-for="(answer, aIndex) in question.answers"
                  :key="aIndex"
                >
                  <v-list-item-action>
                    <v-checkbox
                      v-model="responses[index].selectedAnswers"
                      :value="answer"
                    ></v-checkbox>
                  </v-list-item-action>
                  <v-list-item-content>
                    {{ answer.text }}
                  </v-list-item-content>
                </v-list-item>
              </v-list>
              <v-card-actions>
                <v-btn color="secondary" @click="submitAnswer(index)"
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
import axios from 'axios';

export default {
  data() {
    return {
      questions: [],
      responses: [],
    };
  },
  methods: {
    async loadQuestions() {
      try {
        const response = await axios.get('http://localhost:5133/api/v1/questionnaire/Questionnaire');
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
    submitAnswer(index) {
      // Implement your logic to handle the submission of answers for this specific question
      // You can use this.responses[index] to access the selected answers for this question
    },
    finishQuestionnaire() {
      // Implement your logic for finishing the questionnaire
      // You can use this.responses to access all selected answers
    },
  },
  mounted() {
    this.loadQuestions();
  },
};
</script>