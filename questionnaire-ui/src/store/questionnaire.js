import axios from "axios";

const API_HOST = "questionnaire-service-api:81";

const getDefaultState = () => {
  return {
    respondentId: null,
    respondentName: null,
    respondents: [],
    questions: [],
    responses: [],
    respondentAnswers: [],
    questionnaireScore: {
      score: null,
      normalizedScore: null,
    },
  };
};

const state = getDefaultState();

const mutations = {
  resetState(state) {
    Object.assign(state, getDefaultState());
  },
  setRespondentId(state, respondentId) {
    state.respondentId = respondentId;
  },
  setRespondents(state, respondents) {
    state.respondents = respondents;

    if (state.respondentId) {
      state.respondentName = state.respondents.filter(
        (respondent) => respondent.id == state.respondentId
      )[0].name;
    }
  },
  setQuestions(state, questions) {
    state.questions = questions;
  },
  setResponses(state, responses) {
    state.responses = responses;
  },
  setRespondentAnswers(state, respondentAnswers) {
    state.respondentAnswers = respondentAnswers;
  },
  addSelectedAnswer(state, { questionId, answer }) {
    var response = state.responses.find(
      (response) => response.id == questionId
    );
    response.selectedAnswers.push(answer);
  },
  removeSelectedAnswer(state, { questionId, answer }) {
    var response = state.responses.find(
      (response) => response.id == questionId
    );

    const selectedAnswers = response.selectedAnswers;
    const updatedSelectedAnswers = selectedAnswers.filter(
      (selectedAnswer) => selectedAnswer !== answer
    );

    response.selectedAnswers = updatedSelectedAnswers;
  },
  clearSelectedAnswers(state, { questionId }) {
    var response = state.responses.find(
      (response) => response.id == questionId
    );
    response.selectedAnswers = [];
  },
  setQuestionnaireScore(state, questionnaireScore) {
    state.questionnaireScore.score = questionnaireScore.score;
    state.questionnaireScore.normalizedScore = questionnaireScore.normalizedScore;
  },
};

const actions = {
  async loadRespondents({ commit, state }) {
    try {
      const response = await axios.get(
        `http://${API_HOST}/api/v1/questionnaire/Respondent`
      );

      commit("setRespondents", response.data);
    } catch (error) {
      console.error(error);
    }
  },
  async loadQuestionnaire({ commit, state }) {
    try {
      const response = await axios.get(
        `http://${API_HOST}/api/v1/questionnaire/Questionnaire`
      );

      const questions = response.data[0].questions;

      commit("setQuestions", questions);

      const responses = [];
      for (let i = 0; i < questions.length; i++) {
        const question = questions[i];
        responses.push({ id: question.id, selectedAnswers: [] });
      }

      commit("setResponses", responses);
    } catch (error) {
      console.error(error);
    }
  },
  async loadRespondentAnswers({ commit, state }) {
    try {
      const response = await axios.get(
        `http://${API_HOST}/api/v1/questionnaire/RespondentAnswer?respondentId=${state.respondentId}`
      );

      const respondentAnswers = response.data;

      for (const respondentAnswer of respondentAnswers) {
        var question = state.questions.find(
          (question) => question.id === respondentAnswer.questionId
        );

        if (question) {
          var questionId = question.id;

          var answer = question.answers.find(
            (answer) => answer.id === respondentAnswer.answerId
          );

          if (answer) {
            commit("addSelectedAnswer", { questionId, answer });
          }
        }
      }

      commit("setRespondentAnswers", respondentAnswers);
    } catch (error) {
      console.error("Error fetching respondent answers:", error);
    }
  },
  async updateSelectedAnswers({ commit, state }, { questionId, answer }) {
    debugger;
    var response = state.responses.find(
      (response) => response.id == questionId
    );

    const isSelected = response.selectedAnswers.includes(answer);

    if (isSelected) {
      commit("removeSelectedAnswer", { questionId, answer });
    } else {
      commit("addSelectedAnswer", { questionId, answer });
    }
  },
  async clearAndUpdateSelectedAnswers({ dispatch, commit }, { questionId, answer }) {
    debugger;
    commit("clearSelectedAnswers", { questionId });
    await dispatch("updateSelectedAnswers", { questionId, answer });
  },
  async submitQuestionAnswers({ commit, state }, {questionId, selectedAnswerIds}) {
    const requestBody = {
      id: questionId,
      answerIds: selectedAnswerIds,
    };

    try {
      await axios.put(
        `http://${API_HOST}/api/v1/questionnaire/Question?respondentId=${state.respondentId}`,
        requestBody
      );
      console.log(requestBody);
    } catch (error) {
      console.error("Error submitting question answers:", error);
    }
  },
  async finishQuestionnaire({ commit, state }) {
    try {
      const response = await axios.post(
        `http://${API_HOST}/api/v1/questionnaire/Questionnaire?respondentId=${state.respondentId}`
      );

      commit("setQuestionnaireScore", response.data);
    } catch (error) {
      console.error("Error fetching questionnaire score:", error);
    }
  },
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
};
