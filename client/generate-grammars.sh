#!/bin/sh

# Script for generating textmate grammars from various sources

git clone https://github.com/github/linguist.git

./linguist/script/bootstrap

./linguist/script/build-grammars-tarball

mv linguist/linguist-grammars static/grammars

rm -rf linguist
