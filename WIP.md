This app should provide mermaid text-to-image or text-to-svg conversion as well as a live editor that can store images
or be a general static image host if there isnt one to forward the generated ones to.

## Working so far
- Preview Graph works 
- Render as Png works
- can get output from unit test from the command line
- typescripted, browserified, and gulped

## next up
- needs user file handling
  - "list my graphs"
  - "save work in progress"
 
 
## known broken
 - not broken, but rendering via CLI takes a few seconds 
 - cant get logs from mermaidAPI
 - I have a few issues open at the mermaid project, and a few that I am following. 
   - https://github.com/knsv/mermaid/issues/500
   - https://github.com/knsv/mermaid/issues/495
   - https://github.com/knsv/mermaid/issues/489

## Work in progress
- can edgejs work better for including node modules. maybe, but browserify is fine so far
- needs user file handling
  - "list my graphs"
  - "save work in progress"
  - "share graphs with others"
  - "collab edit"
- add other static image hosting