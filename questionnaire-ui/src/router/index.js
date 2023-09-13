import Vue from "vue";
import VueRouter from "vue-router";
import HomeView from "@/views/HomeView.vue";
import QuestionnaireView from "@/views/QuestionnaireView.vue";
import store from "@/store";

Vue.use(VueRouter);

const routes = [
  {
    path: "/",
    name: "home",
    component: HomeView,
  },
  {
    path: "/:respondentId",
    name: "questionnaire",
    component: QuestionnaireView,
    props: (route) => ({ respondentId: Number(route.params.respondentId) }),
    beforeEnter: (to, from, next) => {
      const respondentId = Number(to.params.respondentId);
      store.commit("questionnaire/resetState");
      store.commit("questionnaire/setRespondentId", respondentId);
      next();
    },
  },
];

const router = new VueRouter({
  mode: "history",
  routes,
});

export default router;
