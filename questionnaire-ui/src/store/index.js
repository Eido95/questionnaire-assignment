import Vue from "vue";
import Vuex from "vuex";
import questionnaire from "@/store/questionnaire";

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    questionnaire,
  },
});
