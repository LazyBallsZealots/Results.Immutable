import { parseStringPromise, Builder } from "xml2js";
import { readFile, writeFile } from "fs/promises";

const builder = new Builder();

const packageJson = await readFile("./package.json");
const pkg = JSON.parse(packageJson.toString());
const projectName = pkg.name;

// Opening the csproj with the same name as the package.json name
const csprojFilePath = `./${projectName}.csproj`;
const csprojXml = await readFile(csprojFilePath);
const csproj = await parseStringPromise(csprojXml);

// Modify the version in the csproj file based on the package.json version
csproj.Project.PropertyGroup[0].Version = pkg.version;

const modified = builder.buildObject(csproj);

await writeFile(csprojFilePath, modified);
console.log(
  `Version updated in ${csprojFilePath} to ${pkg.version} successfully`
);
