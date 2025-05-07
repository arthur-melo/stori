export interface paths {
  '/api/v1/auth/signup': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    get?: never;
    put?: never;
    /**
     * Signs up a user
     * @description Sample request:
     *
     *         POST /api/v1/auth/signup
     *         {
     *           "username": "some-username",
     *           "name": "John doe",
     *           "email": "user@example.com",
     *           "password": "P@ssw0rd",
     *         }
     */
    post: {
      parameters: {
        query?: never;
        header?: never;
        path?: never;
        cookie?: never;
      };
      /** @description User data */
      requestBody: {
        content: {
          'application/json': components['schemas']['SignupRequest'];
        };
      };
      responses: {
        /** @description Returns an empty response when the user is sucessfully created */
        201: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
      };
    };
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/auth/signin': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    get?: never;
    put?: never;
    /**
     * Signs in a user
     * @description Sample request:
     *
     *         POST /api/v1/auth/signin
     *         {
     *           "email": "user@example.com",
     *           "password": "P@ssw0rd",
     *         }
     */
    post: {
      parameters: {
        query?: never;
        header?: never;
        path?: never;
        cookie?: never;
      };
      /** @description User data */
      requestBody: {
        content: {
          'application/json': components['schemas']['SigninRequest'];
        };
      };
      responses: {
        /** @description Returns a JWT access and refresh tokens for authentication */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['TokenResponse'];
          };
        };
        /** @description If the parameters validation/authentication failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given `email` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/auth/token/refresh': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    get?: never;
    put?: never;
    /**
     * Generates a new access token from a given refresh token. A new token pair will be generated.
     * @description Sample request:
     *
     *         POST /api/v1/auth/token/refresh
     *         {
     *           "token": "some-refresh-token",
     *         }
     */
    post: {
      parameters: {
        query?: never;
        header?: never;
        path?: never;
        cookie?: never;
      };
      /** @description User data */
      requestBody: {
        content: {
          'application/json': components['schemas']['RefreshTokenRequest'];
        };
      };
      responses: {
        /** @description Returns a JWT access and refresh tokens for authentication */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['TokenResponse'];
          };
        };
        /** @description If the parameters/credentials validation failed, or if the token is expired */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given `token` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/auth/token/revoke': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    get?: never;
    put?: never;
    post?: never;
    /**
     * Revokes a refresh token
     * @description Sample request:
     *
     *         DELETE /api/v1/auth/token/revoke
     *         {
     *           "token": "some-refresh-token",
     *           "email": "user@example.com",
     *         }
     */
    delete: {
      parameters: {
        query?: never;
        header?: never;
        path?: never;
        cookie?: never;
      };
      /** @description User data */
      requestBody: {
        content: {
          'application/json': components['schemas']['RefreshTokenRequest'];
        };
      };
      responses: {
        /** @description Returns an empty response when the token is revoked */
        204: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters/credentials validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given `token` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/awards': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a list of book awards
     * @description Sample request:
     *
     *         GET /api/v1/awards?pageSize=10&pageNumber=1&name=filterAwardByName
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
          name?: string;
        };
        header?: never;
        path?: never;
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated list of book awards */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['StringPaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/books': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a list of books
     * @description Sample request:
     *
     *         GET /api/v1/books?pageSize=10&pageNumber=1&orderBy=rating&genre=genreFilter&title=titleFilter&character=characterFilter&award=awardFilter&setting=settingFilter
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     *
     *     Valid `orderBy` values: rating or date
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
          genre?: string;
          title?: string;
          character?: string;
          award?: string;
          setting?: string;
          orderBy?: string;
        };
        header?: never;
        path?: never;
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated list of books */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['BookListResponsePaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/books/{id}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a single book by id
     * @description Sample request:
     *
     *         GET /api/v1/books/{id}
     */
    get: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          /** @description Book id */
          id: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a single book data in detailed format */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['BookResponseEnvelope'];
          };
        };
        /** @description If the given `id` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/books/bookId/{bookId}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a single book by book id (String)
     * @description Sample request:
     *
     *         GET /api/v1/books/{bookId}
     */
    get: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          /** @description Book id */
          bookId: string;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a single book data in detailed format */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['BookResponseEnvelope'];
          };
        };
        /** @description If the given `id` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/characters': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a list of book characters
     * @description Sample request:
     *
     *         GET /api/v1/characters?pageSize=10&pageNumber=1&name=filterCharacterByName
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
          name?: string;
        };
        header?: never;
        path?: never;
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated list of book characters */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['StringPaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/genres': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a list of book genres
     * @description Sample request:
     *
     *         GET /api/v1/genres?pageSize=10&pageNumber=1&name=filterGenreByName
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
          name?: string;
        };
        header?: never;
        path?: never;
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated list of book genres */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['StringPaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/readlists/{username}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns the readlist of a given user
     * @description Sample request:
     *
     *         GET /api/v1/readlists/{username}
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
        };
        header?: never;
        path: {
          username: string;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated readlist added by a given `username` */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['ReadlistResponsePaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given username was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/readlists/{username}/{bookId}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a single readlist book from a given user
     * @description Sample request:
     *
     *         GET /api/v1/readlist/{username}/{bookId}
     */
    get: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
          bookId: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a single readlist book by a given `username` */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['ReadlistByBookResponseEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given `username` or `bookId` was not found, or if there is readlist available. */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    /**
     * Adds a book to the readlist of a given user
     * @description Sample request:
     *
     *         POST /api/v1/readlists/{username}/{bookId}
     */
    post: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
          bookId: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns an empty response when the given `bookId` was added to the `username` readlist */
        201: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` or `bookId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    /**
     * Removes a book from the readlist of a given user
     * @description Sample request:
     *
     *         DELETE /api/v1/readlists/{username}/{bookId}
     */
    delete: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
          bookId: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns an empty response when the given `bookId` was deleted from the `username` readlist */
        204: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` or `bookId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/reviews/{username}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns the book reviews of a given user
     * @description Sample request:
     *
     *         GET /api/v1/reviews/{username}
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
        };
        header?: never;
        path: {
          username: string;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated review list in chronological order by a given `username` */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['ReviewResponsePaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given `username` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    post?: never;
    /**
     * Removes a review from the user
     * @description Sample request:
     *
     *         DELETE /api/v1/reviews/{username}
     *         {
     *           "reviewId": 1
     *         }
     */
    delete: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
        };
        cookie?: never;
      };
      /** @description Review data */
      requestBody: {
        content: {
          'application/json': components['schemas']['ReviewRequestCommentBody'];
        };
      };
      responses: {
        /** @description Returns an empty response when a given review is removed from a book */
        204: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` or `reviewId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/reviews/book/{bookId}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns the reviews of a given book
     * @description Sample request:
     *
     *         GET /api/v1/reviews/book/{bookId}
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
        };
        header?: never;
        path: {
          bookId: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated review list in chronological order from a given `bookId` */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['ReviewBookResponsePaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given `bookId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    /**
     * Adds a new review to a given book
     * @description Sample request:
     *
     *         POST /api/v1/reviews/book/{bookId}
     *         {
     *           "text": "This is a sample book comment"
     *         }
     */
    post: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          bookId: number;
        };
        cookie?: never;
      };
      /** @description Review data */
      requestBody: {
        content: {
          'application/json': components['schemas']['ReviewRequestNewCommentBody'];
        };
      };
      responses: {
        /** @description Returns an empty response when a given review is added to a book */
        201: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `bookId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/reviews/patch/{reviewId}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    get?: never;
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    /**
     * Patches a given review
     * @description Sample request:
     *
     *         PATCH /api/v1/reviews/patch/{reviewId}
     *         {
     *           "text": "This is the new book comment"
     *         }
     */
    patch: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          reviewId: number;
        };
        cookie?: never;
      };
      /** @description Review data */
      requestBody: {
        content: {
          'application/json': components['schemas']['ReviewRequestNewCommentBody'];
        };
      };
      responses: {
        /** @description Returns an empty response when a given review is patched */
        201: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed, or if the user does not have permission to edit a review. */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `reviewId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    trace?: never;
  };
  '/api/v1/settings': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a list of book settings
     * @description Sample request:
     *
     *         GET /api/v1/settings?pageSize=10&pageNumber=1&name=filterSettingByName
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
          name?: string;
        };
        header?: never;
        path?: never;
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated list of book settings */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['StringPaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/titles': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a list of book titles
     * @description Sample request:
     *
     *         GET /api/v1/titles?pageSize=10&pageNumber=1&name=filterTitleByName
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
          name?: string;
        };
        header?: never;
        path?: never;
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated list of book titles */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['StringPaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/users': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns the current authorized user data.
     * @description Sample request:
     *
     *         GET /api/v1/users
     */
    get: {
      parameters: {
        query?: never;
        header?: never;
        path?: never;
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns the given authorized user data */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['UserAuthorizedResponseEnvelope'];
          };
        };
        /** @description Bad Request */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/users/{username}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a given user data
     * @description Sample request:
     *
     *         GET /api/v1/users/{username}
     */
    get: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns the given `username` data */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['UserUnauthorizedResponseEnvelope'];
          };
        };
        /** @description Bad Request */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    /**
     * Patches a given user data
     * @description Sample request:
     *
     *         PATCH /api/v1/users/{username}
     *         {
     *           "username": "new-username",
     *           "name": "New name",
     *           "email": "newEmail@example.com",
     *           "password": "newPassword",
     *         }
     *
     *     All body parameters are optional
     */
    patch: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
        };
        cookie?: never;
      };
      /** @description User data */
      requestBody: {
        content: {
          'application/json': components['schemas']['UserPatchRequestBody'];
        };
      };
      responses: {
        /** @description Returns the given `username` updated data */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['UserAuthorizedResponseEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    trace?: never;
  };
  '/api/v1/users/{username}/upload': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    get?: never;
    put?: never;
    /**
     * Uploads a profile image to a given user
     * @description Sample request:
     *
     *         POST /api/v1/users/{username}/upload
     *         {
     *           "profileImg": FormData,
     *         }
     */
    post: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
        };
        cookie?: never;
      };
      requestBody?: {
        content: {
          'multipart/form-data': {
            /** Format: binary */
            profileImg: File;
          };
        };
      };
      responses: {
        /** @description Returns an empty response when the image gets uploaded to the given `username` */
        201: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    /**
     * Removes the photo from the user
     * @description Sample request:
     *
     *         DELETE /api/v1/users/{username}/upload
     */
    delete: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns an empty response when the user photo is removed */
        204: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description Bad Request */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/user_ratings/{username}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns the book ratings of a given user
     * @description Sample request:
     *
     *         GET /api/v1/user_ratings/{username}
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
        };
        header?: never;
        path: {
          username: string;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated user rating list in chronological order by a given `username` */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['UserRatingResponsePaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given `username` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/user_ratings/{username}/{bookId}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a given book rating from a given user
     * @description Sample request:
     *
     *         GET /api/v1/user_ratings/{username}/{bookId}
     */
    get: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
          bookId: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a single book rating by a given `username` */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['UserRatingByBookResponseEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given `username` or `bookId` was not found, or if there is no book rating available. */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    /**
     * Adds a new user rating to a given book
     * @description Sample request:
     *
     *         POST /api/v1/user_ratings/{username}/{bookId}
     *         {
     *           "rating": 5
     *         }
     */
    post: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
          bookId: number;
        };
        cookie?: never;
      };
      /** @description User Rating data */
      requestBody: {
        content: {
          'application/json': components['schemas']['UserRatingRequestBody'];
        };
      };
      responses: {
        /** @description Returns an empty response when a given rating is added to a book */
        201: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` or `bookId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    /**
     * Removes a user rating from the user
     * @description Sample request:
     *
     *         DELETE /api/v1/user_ratings/{username}/{bookId}
     */
    delete: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
          bookId: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns an empty response when a given rating is removed from a book */
        204: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` or `bookId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/wishlists/{username}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns the wishlist of a given user
     * @description Sample request:
     *
     *         GET /api/v1/wishlists/{username}
     *
     *     All query string parameters are optional
     *
     *     Valid `pageSize` values are: 10, 25, 50, 100
     */
    get: {
      parameters: {
        query?: {
          pageSize?: number;
          pageNumber?: number;
        };
        header?: never;
        path: {
          username: string;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a paginated wishlist added by a given `username` */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['WishlistResponsePaginatedListEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given username was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    post?: never;
    delete?: never;
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
  '/api/v1/wishlists/{username}/{bookId}': {
    parameters: {
      query?: never;
      header?: never;
      path?: never;
      cookie?: never;
    };
    /**
     * Returns a single wishlist book from a given user
     * @description Sample request:
     *
     *         GET /api/v1/wishlist/{username}/{bookId}
     */
    get: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
          bookId: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns a single wishlist book by a given `username` */
        200: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/json': components['schemas']['WishlistByBookResponseEnvelope'];
          };
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the given `username` or `bookId` was not found, or if there is wishlist available. */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    put?: never;
    /**
     * Adds a book to the wishlist of a given user
     * @description Sample request:
     *
     *         POST /api/v1/wishlists/{username}/{bookId}
     */
    post: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
          bookId: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns an empty response when the given `bookId` was added to the `username` wishlist */
        201: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` or `bookId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    /**
     * Removes a book from the wishlist of a given user
     * @description Sample request:
     *
     *         DELETE /api/v1/wishlists/{username}/{bookId}
     */
    delete: {
      parameters: {
        query?: never;
        header?: never;
        path: {
          username: string;
          bookId: number;
        };
        cookie?: never;
      };
      requestBody?: never;
      responses: {
        /** @description Returns an empty response when the given `bookId` was deleted from the `username` wishlist */
        204: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the parameters validation failed */
        400: {
          headers: {
            [name: string]: unknown;
          };
          content: {
            'application/problem+json': components['schemas']['HttpValidationProblemDetails'];
          };
        };
        /** @description If the authentication failed */
        401: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
        /** @description If the given `username` or `bookId` was not found */
        404: {
          headers: {
            [name: string]: unknown;
          };
          content?: never;
        };
      };
    };
    options?: never;
    head?: never;
    patch?: never;
    trace?: never;
  };
}
export type webhooks = Record<string, never>;
export interface components {
  schemas: {
    AccessToken: {
      token: string;
      /** Format: int64 */
      expiration: number;
    };
    BookListRatingResponse: {
      /** Format: double */
      starsAverage: number;
      /** Format: int32 */
      starsTotal: number;
    };
    BookListResponse: {
      /** Format: int32 */
      id: number;
      bookId: string;
      title: string;
      description?: string | null;
      publishDate?: string | null;
      coverImg: string;
      rating?: components['schemas']['BookListRatingResponse'] | null;
    };
    BookListResponsePaginatedListEnvelope: {
      data: components['schemas']['BookListResponse'][];
      /** Format: int32 */
      pageNumber: number;
      /** Format: int32 */
      totalPages: number;
      /** Format: int32 */
      totalItems: number;
      readonly hasPreviousPage: boolean;
      readonly hasNextPage: boolean;
    };
    BookResponse: {
      /** Format: int32 */
      id: number;
      bookId: string;
      title: string;
      series?: string | null;
      description?: string | null;
      language?: string | null;
      isbn: string;
      bookFormat?: string | null;
      edition?: string | null;
      /** Format: int32 */
      pages?: number | null;
      publishDate?: string | null;
      coverImg: string;
      publisher?: string | null;
      rating?: components['schemas']['RatingResponse'] | null;
      awards: string[];
      characters: string[];
      genres: string[];
      settings: string[];
    };
    BookResponseEnvelope: {
      data: components['schemas']['BookResponse'][];
    };
    HttpValidationProblemDetails: {
      type?: string | null;
      title?: string | null;
      /** Format: int32 */
      status?: number | null;
      detail?: string | null;
      instance?: string | null;
      errors: {
        [key: string]: string[];
      };
    } & {
      [key: string]: unknown;
    };
    RatingResponse: {
      /** Format: int32 */
      star1: number | null;
      /** Format: int32 */
      star2: number | null;
      /** Format: int32 */
      star3: number | null;
      /** Format: int32 */
      star4: number | null;
      /** Format: int32 */
      star5: number | null;
      /** Format: double */
      starsAverage: number | null;
      /** Format: int32 */
      starsTotal: number | null;
    };
    ReadlistByBookResponse: {
      book: components['schemas']['BookListResponse'];
      /** Format: date-time */
      createdAt: string;
    };
    ReadlistByBookResponseEnvelope: {
      data: components['schemas']['ReadlistByBookResponse'][];
    };
    ReadlistResponse: {
      book: components['schemas']['BookListResponse'];
      /** Format: date-time */
      createdAt: string;
    };
    ReadlistResponsePaginatedListEnvelope: {
      data: components['schemas']['ReadlistResponse'][];
      /** Format: int32 */
      pageNumber: number;
      /** Format: int32 */
      totalPages: number;
      /** Format: int32 */
      totalItems: number;
      readonly hasPreviousPage: boolean;
      readonly hasNextPage: boolean;
    };
    RefreshToken: {
      token: string;
      /** Format: int64 */
      expiration: number;
    };
    RefreshTokenRequest: {
      token: string | null;
    };
    ReviewBookResponse: {
      /** Format: int32 */
      id: number;
      author: components['schemas']['ReviewBookUserResponse'];
      text: string;
      /** Format: date-time */
      createdAt: string;
      /** Format: int32 */
      rating?: number | null;
    };
    ReviewBookResponsePaginatedListEnvelope: {
      data: components['schemas']['ReviewBookResponse'][];
      /** Format: int32 */
      pageNumber: number;
      /** Format: int32 */
      totalPages: number;
      /** Format: int32 */
      totalItems: number;
      readonly hasPreviousPage: boolean;
      readonly hasNextPage: boolean;
    };
    ReviewBookUserResponse: {
      username: string;
      name: string;
      profileImg?: string | null;
    };
    ReviewRequestCommentBody: {
      /** Format: int32 */
      reviewId: number | null;
    };
    ReviewRequestNewCommentBody: {
      text: string | null;
    };
    ReviewResponse: {
      /** Format: int32 */
      id: number;
      book: components['schemas']['BookListResponse'];
      text: string;
      /** Format: date-time */
      createdAt: string;
      /** Format: int32 */
      rating?: number | null;
    };
    ReviewResponsePaginatedListEnvelope: {
      data: components['schemas']['ReviewResponse'][];
      /** Format: int32 */
      pageNumber: number;
      /** Format: int32 */
      totalPages: number;
      /** Format: int32 */
      totalItems: number;
      readonly hasPreviousPage: boolean;
      readonly hasNextPage: boolean;
    };
    SigninRequest: {
      email: string | null;
      password: string | null;
    };
    SignupRequest: {
      username: string | null;
      name: string | null;
      email: string | null;
      password: string | null;
    };
    StringPaginatedListEnvelope: {
      data: string[];
      /** Format: int32 */
      pageNumber: number;
      /** Format: int32 */
      totalPages: number;
      /** Format: int32 */
      totalItems: number;
      readonly hasPreviousPage: boolean;
      readonly hasNextPage: boolean;
    };
    TokenResponse: {
      accessToken: components['schemas']['AccessToken'] | null;
      refreshToken: components['schemas']['RefreshToken'] | null;
    };
    UserAuthorizedResponse: {
      username: string;
      email: string;
      name: string;
      profileImg?: string | null;
      /** Format: date-time */
      createdAt: string;
    };
    UserAuthorizedResponseEnvelope: {
      data: components['schemas']['UserAuthorizedResponse'][];
    };
    UserPatchRequestBody: {
      email: string | null;
      password: string | null;
      username: string | null;
      name: string | null;
    };
    UserRatingByBookResponse: {
      /** Format: int32 */
      rating: number;
    };
    UserRatingByBookResponseEnvelope: {
      data: components['schemas']['UserRatingByBookResponse'][];
    };
    UserRatingRequestBody: {
      /** Format: int32 */
      rating: number | null;
    };
    UserRatingResponse: {
      book: components['schemas']['BookListResponse'];
      /** Format: int32 */
      rating: number;
      /** Format: date-time */
      createdAt: string;
    };
    UserRatingResponsePaginatedListEnvelope: {
      data: components['schemas']['UserRatingResponse'][];
      /** Format: int32 */
      pageNumber: number;
      /** Format: int32 */
      totalPages: number;
      /** Format: int32 */
      totalItems: number;
      readonly hasPreviousPage: boolean;
      readonly hasNextPage: boolean;
    };
    UserUnauthorizedResponse: {
      username: string;
      name: string;
      profileImg?: string | null;
      /** Format: date-time */
      createdAt: string;
    };
    UserUnauthorizedResponseEnvelope: {
      data: components['schemas']['UserUnauthorizedResponse'][];
    };
    WishlistByBookResponse: {
      book: components['schemas']['BookListResponse'];
      /** Format: date-time */
      createdAt: string;
    };
    WishlistByBookResponseEnvelope: {
      data: components['schemas']['WishlistByBookResponse'][];
    };
    WishlistResponse: {
      book: components['schemas']['BookListResponse'];
      /** Format: date-time */
      createdAt: string;
    };
    WishlistResponsePaginatedListEnvelope: {
      data: components['schemas']['WishlistResponse'][];
      /** Format: int32 */
      pageNumber: number;
      /** Format: int32 */
      totalPages: number;
      /** Format: int32 */
      totalItems: number;
      readonly hasPreviousPage: boolean;
      readonly hasNextPage: boolean;
    };
  };
  responses: never;
  parameters: never;
  requestBodies: never;
  headers: never;
  pathItems: never;
}
export type $defs = Record<string, never>;
export type operations = Record<string, never>;
