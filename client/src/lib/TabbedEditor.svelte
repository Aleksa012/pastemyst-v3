<script lang="ts">
    import { onMount, tick } from "svelte";
    import Sortable, { type SortableEvent } from "sortablejs";
    import Tab from "./Tab.svelte";
    import Editor from "./Editor.svelte";
    import TabData from "./TabData";

    export let tabs: TabData[] = new Array<TabData>();
    export let activeTab: TabData | undefined = undefined;
    $: activeTab = tabs.find((t) => t.id === activeTabId);

    // used for giving tabs their own unique ID
    let tabCounter = 0;

    let tabGroupElement: HTMLElement;
    let editorTarget: HTMLElement;

    let activeTabId = 0;

    onMount(async () => {
        Sortable.create(tabGroupElement, {
            direction: "horizontal",
            animation: 150,
            delay: 50,

            onEnd: (event: SortableEvent) => {
                // once the reordering of tabs is done, replicate the reorder in the data array
                if (event.oldIndex && event.newIndex) {
                    const tab = tabs[event.oldIndex];
                    tabs.splice(event.oldIndex, 1);
                    tabs.splice(event.newIndex, 0, tab);
                    tabs = tabs;
                }
            }
        });

        await addTab();
    });

    const onTabClose = async (id: number) => {
        // cant close last tab
        if (tabs.length === 1) return;

        const idx = tabs.findIndex((t) => t.id === id);

        // ask if it's okay to close a non empty tab
        if (tabs[idx].editor.getContent().length > 0) {
            if (!confirm("Are you sure you want to close a non-empty tab?")) {
                return;
            }
        }

        // destroy editor element
        tabs[idx].editor.$destroy();

        // remove from array
        tabs = [...tabs.slice(0, idx), ...tabs.slice(idx + 1, tabs.length)];

        if (tabs.findIndex((t) => t.id === activeTabId) >= 1) {
            // set the previous tab as active
            await setActiveTab(tabs[tabs.length - 1].id);
        } else {
            // if the first tab is closed, set the next tab as active
            await setActiveTab(tabs[0].id);
        }

        updateTabEditorVisibility();
    };

    // eslint-disable-next-line @typescript-eslint/no-explicit-any
    const onTabClick = async (id: number, event: CustomEvent<any>) => {
        let target = event.detail.event.target as HTMLElement;

        // ignore if close icon is pressed
        if (target.nodeName === "SVG") return;

        // ignore if double click, so the rename field doesn't unfocus
        if (event.detail.event.detail > 1) return;

        await setActiveTab(id);
    };

    const onTabFinishRenaming = (id: number) => {
        const idx = tabs.findIndex((t) => t.id === id);

        tabs[idx].editor.focus();
    };

    const addTab = async () => {
        const name = "untitled";

        let newtab = new TabData(tabCounter, name, new Editor({ target: editorTarget }));

        // if a tab already exists, set the new tab's language as the previous one
        if (tabs.length > 0) {
            newtab.editor.setSelectedLang(tabs[tabs.length - 1].editor.getSelectedLang());
        }

        await tick();

        tabs = [...tabs, newtab];
        await setActiveTab(tabs[tabs.length - 1].id);

        tabCounter++;
    };

    const setActiveTab = async (id: number) => {
        activeTabId = id;

        updateTabEditorVisibility();

        // for some reason requires 2 ticks to pass for the editor to be focused correctly
        await tick();
        await tick();

        let tab = tabs.find((t) => t.id === activeTabId);

        if (tab && !tab.isInRenamingState) {
            tab.editor.focus();
        }
    };

    const updateTabEditorVisibility = () => {
        for (let tab of tabs) {
            tab.editor.$set({ hidden: !(tab.id === activeTabId) });
        }
    };
</script>

<div class="tabs flex row center">
    <div class="tabgroup flex row" bind:this={tabGroupElement}>
        {#each tabs as tab, _ (tab.id)}
            <Tab
                id={tab.id.toString()}
                on:close={() => onTabClose(tab.id)}
                on:click={(event) => onTabClick(tab.id, event)}
                on:finishedRenaming={() => onTabFinishRenaming(tab.id)}
                bind:isInRenamingState={tab.isInRenamingState}
                bind:title={tab.title}
                isActive={activeTabId === tab.id}
                closeable={tabs.length > 1}
            />
        {/each}
    </div>

    <div class="add-btn btn" on:click={addTab}>
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" class="icon">
            <title>Plus Icon</title>
            <path
                fill="currentColor"
                fill-rule="evenodd"
                d="M7.75 2a.75.75 0 01.75.75V7h4.25a.75.75 0 110 1.5H8.5v4.25a.75.75 0 11-1.5 0V8.5H2.75a.75.75 0 010-1.5H7V2.75A.75.75 0 017.75 2z"
            />
        </svg>
    </div>
</div>

<div class="editor" bind:this={editorTarget} />

<style lang="scss">
    .tabs {
        width: 100%;
        box-sizing: border-box;
        background-color: var(--color-bg2);
        border-radius: $border-radius $border-radius 0 0;
        margin-bottom: -1px; // collapse bottom border

        .tabgroup {
            flex-wrap: wrap;
        }

        .add-btn {
            margin: 0 0.5rem;
            border-bottom-color: transparent;
            background-color: transparent;

            &:hover {
                border-bottom-color: var(--color-bg3);
            }

            .icon {
                max-width: 15px;
                max-height: 15px;
            }
        }
    }

    .tabs .tabgroup :global(.sortable-drag) {
        opacity: 0;
    }

    .tabs .tabgroup :global(.sortable-chosen) {
        @include shadow-big();
        z-index: 10;
        border-color: var(--color-secondary);
    }
</style>
