import axios from "axios";

const state = {
  respondentId: 1,
  questions: [],
  responses: [],
  respondentAnswers: [],
};

const mutations = {
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
};

const actions = {
  async loadRespondentAnswers({ commit, state }) {
    // eslint-disable-next-line no-undef
    axios
      .get(
        `http://localhost:5133/api/v1/questionnaire/RespondentAnswer?respondentId=${state.respondentId}`
      )
      .then((response) => {
        var respondentAnswers = response.data;

        // Loop through respondentAnswers
        respondentAnswers.forEach((answer) => {
          // Find the corresponding question
          const questionIndex = state.questions.findIndex(
            (q) => q.id === answer.questionId
          );
          if (questionIndex !== -1) {
            // Find the corresponding answer
            const answerIndex = state.questions[
              questionIndex
            ].answers.findIndex((a) => a.id === answer.answerId);
            if (answerIndex !== -1) {
              // Check the corresponding checkbox
              state.responses[questionIndex].selectedAnswers.push(
                state.questions[questionIndex].answers[answerIndex]
              );
            }
          }
        });

        commit("setRespondentAnswers", respondentAnswers);
      })
      .catch((error) => {
        console.error("Error fetching respondent answers:", error);
      });
  },
  async loadQuestionnaire({ commit, state }) {
    try {
      const response = await axios.get(
        "http://localhost:5133/api/v1/questionnaire/Questionnaire"
      );
      const questionnaireData = response.data[0];

      var questions = questionnaireData.questions.map((question) => ({
        id: question.id,
        text: question.text,
        answers: question.answers.map((answer) => ({
          id: answer.id,
          text: answer.text,
        })),
      }));

      commit("setQuestions", questions);

      var responses = Array.from({ length: questions.length }, () => ({
        selectedAnswers: [],
      }));

      commit("setResponses", responses);
    } catch (error) {
      console.error(error);
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
    debugger;
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
