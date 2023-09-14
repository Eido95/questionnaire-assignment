<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols="12" sm="8" md="6">
        <v-img
          src="https://upload.wikimedia.org/wikipedia/commons/thumb/2/2a/Mastercard-logo.svg/1280px-Mastercard-logo.svg.png"
          class="my-3"
          contain
          height="100"
        />
        <v-card class="mx-auto mt-10" max-width="600">
          <v-card-title class="text-h2">Questionnaire</v-card-title>
          <v-card-subtitle class="text-h5"
            >Respondents > {{ respondentName }}</v-card-subtitle
          >
          <v-card-text>
            <p>
              The questionnaire is based on the following links:<br/>
              1. <a href="https://www.mastercardservices.com/en/solutions/security-business-operations/cyber-quant">
                Cyber Quant | Mastercard Data & Services</a>
                <br>
              2. <a href="https://www.mastercard.ca/en-ca/business/large-enterprise/safety-security/cyber-solutions/cyber-quant.html">
                Cyber Quant | Mastercard's Cybersecurity Risk Assessment</a>
                <br>
              <br/>
              Click "Finish" button below to see your quetionnaire score </br> 
              (don't forget to submit all your answers)
            </p>
            <v-row>
              <v-col>
                <v-select 
                class="mx-auto"
                v-model="filter"
                :items="filterOptions"
                label="Filter"
                ></v-select>
              </v-col>
              <v-spacer></v-spacer>
            </v-row>
            <v-card
              v-for="(question, questionIndex) in filteredQuestions"
              :index="questionIndex"
              :key="question.id"
              class="ma-5"
            >
              <v-card-title class="text-subtitle-1" style="word-break: keep-all;">
                Question: {{ question.text }}
              </v-card-title>
              <v-card-subtitle v-if="question.comment">
                Comment: {{ question.comment }}
              </v-card-subtitle>
              <v-list v-if="question.comment">
                <v-list-item
                  v-for="(answer, answerIndex) in question.answers"
                  :index="answerIndex"
                  :key="answer.id"
                >
                  <v-list-item-action>
                    <v-simple-checkbox
                      :value="isSelected(question.id, answer)"
                      @click="updateSelectedAnswers({questionId: question.id, answer})"
                      color="primary"
                    ></v-simple-checkbox>
                  </v-list-item-action>
                  <v-list-item-content>
                    {{ answer.text }}
                  </v-list-item-content>
                </v-list-item>
              </v-list>
              <v-list v-else>
                <v-radio-group 
                :value="selectedAnswerId(question.id)"> 
                  <v-list-item
                    v-for="(answer, answerIndex) in question.answers"
                    :index="answerIndex"
                    :key="answer.id">
                    <v-list-item-action>
                      <v-radio
                      :value="answer.id"
                      @click="clearAndUpdateSelectedAnswers({questionId: question.id, answer})"
                      ></v-radio>
                    </v-list-item-action>
                    <v-list-item-content>
                      {{ answer.text }}
                    </v-list-item-content>
                  </v-list-item>
                </v-radio-group>
              </v-list>
              <v-card-actions>
                <v-btn color="secondary" @click="submitQuestion(question.id)">Submit</v-btn>
              </v-card-actions>
            </v-card>
          </v-card-text>
          <v-card-actions>
            <v-btn color="primary" @click="finishQuestionnaire">Finish</v-btn>
            <v-spacer></v-spacer>
            <v-avatar
              v-if="questionnaireScore.normalizedScore !== null"
              class="font-weight-bold text-decoration-underline white--text elevation-12"
              :color="getScoreAvatarColor(questionnaireScore.normalizedScore)"
              size="56"
            >
              {{ questionnaireScore.normalizedScore }}
            </v-avatar>
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
    return {
      filter: 'all',
      filterOptions: [
        { text: 'All', value: 'all' },
        { text: 'Answered', value: 'answered' },
        { text: 'Unanswered', value: 'unanswered' },
      ],
    };
  },
  computed: {
    ...mapState("questionnaire", [
      "questions",
      "responses",
      "respondentAnswers",
      "respondentId",
      "respondentName",
      "questionnaireScore",
    ]),
    filteredQuestions() {
      if (this.filter === 'all') {
        return this.questions;
      } else {
        return this.questions.filter(question => { 
          const questionId = question.id;
          var response = this.responses.find((response) => (response.id == questionId));
          const isAnswered = response && response.selectedAnswers.length > 0;

          if (this.filter === 'answered') {
            return isAnswered;
          } else {
            return !isAnswered
          }
        });
      }
    },
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
      "clearAndUpdateSelectedAnswers",
      "submitQuestionAnswers",
      "finishQuestionnaire",
    ]),
    selectedAnswerId(questionId) {
      var response = this.responses.find((response) => (response.id == questionId));

      if (response.selectedAnswers.length > 0) {
        return response.selectedAnswers[0].id;
      }
      else {
        return null;
      }
    },
    isSelected(questionId, answer) {
      var response = this.responses.find((response) => (response.id == questionId));

      if (response != undefined) {
        return response.selectedAnswers.includes(answer);
      }
    },
    async submitQuestion(questionId) {
      var response = this.responses.find((response) => (response.id == questionId));

      const selectedAnswers = response.selectedAnswers;
      const selectedAnswerIds = selectedAnswers.map((answer) => answer.id);

      this.submitQuestionAnswers({ questionId, selectedAnswerIds });
    },
    getScoreAvatarColor(normalizedScore) {
      if (normalizedScore >= 7) {
        return "primary";
      } else if (normalizedScore >= 4) {
        return "secondary";
      } else {
        return "accent";
      }
    },
  },
  created() {
    this.loadRespondents();
    this.loadQuestionnaire();
    this.loadRespondentAnswers();
  },
};
</script>
