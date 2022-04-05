# CHANGELOG

All notable changes for Desonity package will be documented in this file

## [v0.0.6](https://github.com/desonity/desonity/releases/tag/v0.0.6) - 2022-04-05

This release contains most common functions that might be useful for integrating deso with unity games. I hope future versions will be stable from now on and no breaking changes will be done. I will keep adding more endpoints support (Feel free to make a PR if you feel like adding support for an endpoint).

### Changed

- Renamed source files and moved methods according to official deso docs hierarchy. e.g. profile functions under `Users.cs`, post functions under `Posts.cs` etc.
- `Helpers/Objects` contains classes for [deso data types](https://docs.deso.org/for-developers/backend/blockchain-data/basics/data-types)

### Added

- `createNFT` function.
- `getPostsForPublicKey` function.

### Removed

- Examples folder.

### TODO

- write docs fr this time.

## 0.0.5 - 2022-04-04

Users dont have to necessarily use the JObject anymore. Instead all API keys and values can be now accessed directly through class member variables.

### Changed

- `Desonity.Profile` is a static class now and can be used without instanciating a new object.
- `Desonity.Route` is also a static class now.
- `Profile.getProfile` now returns a `ProfileEntry` object.
- `Profile.submitPost` and `Profile.getPost` new returns a `PostEntry` object.

### Added

- `Runtime/Helpers` folder to contain helper functions. `Helpers/Objects` contains Classes for DeSo API responses (You'll get what I mean after looking at the classes in Objects folder), all functions will be returning one of these classes according to use.

### TODO

- Add Object classes just like profile, in nft too.
- **PLEASE WRITE [DOCUMENTATION](https://desonity.github.io/docs) FFS**.

## 0.0.4 - 2022-04-03

### Changed

- Reaplaced string json responses of POST methods with a standard Response object containing `JObject`, `JArray` and status code for the response. The response data is now easily accessible using `Newtonsoft.Json`.
- Replaced all `PublicKeyBase58Check`s with an identity object

### Added

- Some commonly used variables to `Profile` class, so users dont have to use the JObject everytime.
- profile and nft examples.
- *Identities everywhere*

## 0.0.3 - 2022-03-31

### Changed

- **Massive Changes**, Replaced `IEnumerator`s with `async/await`s.
- `Route.POST(...)` now returns a JSON string of the format `{"Response":data}` if the post request was successful or `{"Error":error}` if it was not successful.

### Added

- Setup `Identity` so users can create an identity object for whatever public key they want and pass this object to other classes like `Profile` and `Nft` to use their functions.
- Users can now make posts to DeSo using `Profile.createPost(...)`.
- Signing of transactions will be done through the Flask Backend until someone figures out how to do the signing on the Unity side in C# (help me pwease :'|)

### TODO

- Replace current string JSON response of `Route.POST(...)` with a more easily useable Object thingy idk.
- Add code for `READ_WRITE_IDENTITY` scope to use Identity window for signing transactions on the users side.

## 0.0.2 - 2022-03-27

### Changed

- Added Serializable Classes for each deso endpoint under `Endpoints.cs`, this will be used to create the Json string that will be posted to the backend. This will make the code more readable and easier to modify the post json.
- Replaced UnityEngines JsonUtility with Newtonsoft.Json to Serialize endpoint classes in `Endpoint.cs`

## 0.0.1 - 2022-03-25

### Added

- Deso Identity Login

### Fixed

- Fixed minor bug in `Profile.getNftsForUser` which did not have nullable bool as a parameter

## 0.0.0 - 2022-03-24

### Added

- get Profile
- get owned NFTs
- get Single NFT
- get avatar url
