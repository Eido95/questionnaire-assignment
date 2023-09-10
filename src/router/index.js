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
      store.commit("questionnaire/setRespondentId", respondentId);
      next();
    },
  },
  {
    path: "/about",
    name: "about",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/AboutView.vue"),
  },
];

const router = new VueRouter({
  mode: "history",
  routes,
});

export default router;
