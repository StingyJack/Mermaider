# Mermaider
An Asp.net core (Mermaid)[https://github.com/knsv/mermaid] live editor.

I needed a learning project for some stuff, and something that could be used to perma-store charts and other images (without copying crazy urls).

It mostly works. There is at least (one upstream issue)[https://github.com/knsv/mermaid/issues/478] that could end up being my fault but I didnt spend too much time on it. The second 
render/preview attempt results in an odd error.

Taking the whole bootstrap function set hoses the generated svg previews, so the gulpfile recompiles only the ones necessary.

Check the [WIP.md](https://github.com/StingyJack/Mermaider/blob/master/WIP.md) for more info.
