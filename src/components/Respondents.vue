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
          <v-card-subtitle class="text-h6">Respondents</v-card-subtitle>
          <v-card-text>
            <v-row justify="">
              <v-col
                ><v-btn color="primary" @click="navigateToRespondent(1)"
                  >1</v-btn
                ></v-col
              >
              <v-col
                ><v-btn color="primary" @click="navigateToRespondent(2)"
                  >2</v-btn
                ></v-col
              >
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
export default {
  name: "respondents",
  methods: {
    navigateToRespondent(respondentId) {
      this.$router.push({ name: "questionnaire", params: { respondentId } });
    },
  },
};
</script>
