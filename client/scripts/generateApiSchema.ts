import fs from 'node:fs';
import { resolve } from 'node:path';
import openapiTS, { astToString } from 'openapi-typescript';
import ts from 'typescript';

const BACKEND_SWAGGER_ENDPOINT = 'http://localhost:8081';

const FILE = ts.factory.createTypeReferenceNode(
  ts.factory.createIdentifier('File'),
);
const NULL = ts.factory.createLiteralTypeNode(ts.factory.createNull());

const path = resolve('src', 'typings', 'api.d.ts');

const run = async () => {
  const ast = await openapiTS(
    new URL('/swagger/v1/swagger.json', BACKEND_SWAGGER_ENDPOINT),
    {
      propertiesRequiredByDefault: true,
      transform(schemaObject) {
        if (schemaObject.format === 'binary') {
          return schemaObject.nullable
            ? ts.factory.createUnionTypeNode([FILE, NULL])
            : FILE;
        }
        return undefined;
      },
    },
  );
  const contents = astToString(ast);

  fs.writeFileSync(path, contents);
};

run()
  .then(() => console.log(`API Schema written to: '${path}'`))
  .catch(err => {
    throw err;
  });
