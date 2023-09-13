import Vue from "vue";
import Vuetify from "vuetify/lib/framework";

Vue.use(Vuetify);

export default new Vuetify({
  theme: {
    themes: {
      light: {
        primary: "#cf4500",
        secondary: "#f38b00",
        accent: "#ffc61e",
      },
    },
  },
});
