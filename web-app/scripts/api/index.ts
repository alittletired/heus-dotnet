import http from "http";
import path from "path";
import { ApiConfig } from "./api";
import ApiCodeGen from "./ApiCodeGen";

let defaultConfig = {
  mapTypes: { Timestamp: "number", Int64: "string" },
  codegenType: "ts",
  pathSplitIndex: 2,
  basePath: "",
  outputDir: "src/api",
  ignoreTypes: ["QueryMap", "PageList", "PageRequest"],
};
let config = require(path.resolve("apiconfig.json"));
let { apis = [], ...restConfig } = { ...defaultConfig, ...config };

for (let api of apis) {
  let config = { ...restConfig, ...api };
  http.get(config.url, (res) => {
    let todo = "";

    // called when a data chunk is received.
    res.on("data", (chunk) => {
      todo += chunk;
    });

    // called when the complete response is received.
    res.on("end", () => {
      console.log(JSON.parse(todo).title);
    });

    res.on("error", (error) => {
      console.log("Error: " + error.message);
    });
  });
}
