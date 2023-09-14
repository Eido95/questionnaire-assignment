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
        <v-card class="mx-auto mt-10" max-width="540">
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
            <v-card
              v-for="question in questions"
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
                  v-for="answer in question.answers"
                  :key="answer.id"
                >
                  <v-list-item-action>
                    <v-checkbox
                      :value="isSelected(question.id, answer)"
                      @change="updateSelectedAnswers({questionId: question.id, answer})"
                    ></v-checkbox>
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
                    v-for="answer in question.answers"
                    :key="answer.id">
                    <v-list-item-action>
                      <v-radio
                      :value="answer.id"
                      @change="clearAndUpdateSelectedAnswers({questionId: question.id, answer})"
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
    return {};
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
      if (this.responses[questionId].selectedAnswers.length > 0) {
        return this.responses[questionId].selectedAnswers[0].id;
      }
      else {
        return null;
      }
    },
    isSelected(questionId, answer) {
      if (this.responses[questionId] != undefined) {
        return this.responses[questionId].selectedAnswers.includes(answer);
      }
    },
    async submitQuestion(questionId) {
      const selectedAnswers = this.responses[questionId].selectedAnswers;
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
