import axios from "axios";

const getDefaultState = () => {
  return {
    respondentId: null,
    respondentName: null,
    respondents: [],
    questions: [],
    responses: [],
    respondentAnswers: [],
  };
};

const state = getDefaultState();

const mutations = {
  setRespondentId(state, respondentId) {
    state.respondentId = respondentId;
  },
  setRespondents(state, respondents) {
    state.respondents = respondents;
    state.respondentName = state.respondents.filter(
      (respondent) => respondent.id == state.respondentId
    )[0].name;
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
  addSelectedAnswer(state, { questionIndex, answer }) {
    state.responses[questionIndex].selectedAnswers.push(answer);
  },
  removeSelectedAnswer(state, { questionIndex, answer }) {
    const selectedAnswers = state.responses[questionIndex].selectedAnswers;
    const updatedSelectedAnswers = selectedAnswers.filter(
      (selectedAnswer) => selectedAnswer !== answer
    );

    state.responses[questionIndex].selectedAnswers = updatedSelectedAnswers;
  },
  resetState(state) {
    Object.assign(state, getDefaultState());
  },
};

const actions = {
  async loadRespondents({ commit, state }) {
    const response = await axios.get(
      "http://localhost:5133/api/v1/questionnaire/Respondent"
    );

    commit("setRespondents", response.data);
  },
  async loadQuestionnaire({ commit, state }) {
    try {
      const response = await axios.get(
        "http://localhost:5133/api/v1/questionnaire/Questionnaire"
      );

      const questions = response.data[0].questions;

      commit("setQuestions", questions);

      const responses = [];
      for (let i = 0; i < questions.length; i++) {
        responses.push({ selectedAnswers: [] });
      }

      commit("setResponses", responses);
    } catch (error) {
      console.error(error);
    }
  },
  async loadRespondentAnswers({ commit, state }) {
    try {
      const response = await axios.get(
        `http://localhost:5133/api/v1/questionnaire/RespondentAnswer?respondentId=${state.respondentId}`
      );

      const respondentAnswers = response.data;

      for (const answer of respondentAnswers) {
        const questionIndex = state.questions.findIndex(
          (q) => q.id === answer.questionId
        );

        if (questionIndex !== -1) {
          const answerIndex = state.questions[questionIndex].answers.findIndex(
            (a) => a.id === answer.answerId
          );

          if (answerIndex !== -1) {
            state.responses[questionIndex].selectedAnswers.push(
              state.questions[questionIndex].answers[answerIndex]
            );
          }
        }
      }

      commit("setRespondentAnswers", respondentAnswers);
    } catch (error) {
      console.error("Error fetching respondent answers:", error);
    }
  },
  async updateSelectedAnswers({ commit, state }, { questionIndex, answer }) {
    const isSelected = state.responses[questionIndex].selectedAnswers.includes(answer);

    if (isSelected) {
      commit("removeSelectedAnswer", { questionIndex, answer });
    } else {
      commit("addSelectedAnswer", { questionIndex, answer });
    }
  },
  async submitQuestionAnswers({ commit, state }, {questionId, selectedAnswerIds}) {
    const requestBody = {
      id: questionId,
      answerIds: selectedAnswerIds,
    };

    try {
      await axios.put(
        `http://localhost:5133/api/v1/questionnaire/Question?respondentId=${state.respondentId}`,
        requestBody
      );
      console.log(requestBody);
    } catch (error) {
      console.error("Error submitting question answers:", error);
    }
  },
};

export default {
  namespaced: true,
  state,
  mutations,
  actions,
};
