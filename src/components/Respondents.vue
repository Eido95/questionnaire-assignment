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
          <v-card-subtitle class="text-h5">Respondents</v-card-subtitle>
          <v-card-text>
            <v-row>
              <v-col
                v-for="respondent in respondents"
                :key="respondent.id"
                class="ma-5"
              >
                <v-btn
                  color="primary"
                  @click="navigateToRespondent(respondent.id)"
                >
                  {{ respondent.name }}</v-btn
                >
              </v-col>
              <v-spacer></v-spacer>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapState, mapMutations, mapActions } from "vuex";

export default {
  name: "respondents",
  computed: {
    ...mapState("questionnaire", ["respondents"]),
  },
  methods: {
    ...mapActions("questionnaire", ["loadRespondents"]),

    navigateToRespondent(respondentId) {
      this.$router.push({ name: "questionnaire", params: { respondentId } });
    },
  },
  created() {
    this.loadRespondents();
  },
};
</script>
