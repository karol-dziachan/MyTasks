/* eslint-disable */
import React from "react";
import config from "../config.json"

const useConfigData = (key) => config[process.env.NODE_ENV][key]

export default useConfigData;