//go:build js && wasm
// +build js,wasm

package main

import (
	"github.com/JohannesKaufmann/html-to-markdown/v2/converter"
	"github.com/JohannesKaufmann/html-to-markdown/v2/plugin/base"
	"github.com/JohannesKaufmann/html-to-markdown/v2/plugin/commonmark"
	"github.com/JohannesKaufmann/html-to-markdown/v2/plugin/table"
	"syscall/js"
)

func HtmlToMd(html string) (string, error) {
	conv := converter.NewConverter(
		converter.WithPlugins(
			base.NewBasePlugin(),
			table.NewTablePlugin(table.WithHeaderPromotion(true)),
			commonmark.NewCommonmarkPlugin(
				commonmark.WithLinkEmptyHrefBehavior(commonmark.LinkBehaviorSkip),
				commonmark.WithLinkEmptyContentBehavior(commonmark.LinkBehaviorSkip)),
		),
	)

	markdown, err := conv.ConvertString(html)
	if err != nil {
		return "", err
	}
	return markdown, nil

}

func main() {
	js.Global().Set("html2md", js.FuncOf(func(this js.Value, args []js.Value) any {
		md, err := HtmlToMd(args[0].String())
		if err != nil {
			return "e" + err.Error()
		}
		return "m" + md
	}))

	<-make(chan struct{})
}
