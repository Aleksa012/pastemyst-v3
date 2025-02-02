<script lang="ts">
    import { ExpiresIn, type Paste } from "$lib/api/paste";
    import type { PageData } from "./$types";
    import { tooltip } from "$lib/tooltips";
    import moment from "moment";
    import { PUBLIC_API_BASE } from "$env/static/public";

    export let data: PageData;

    const getPasteLangs = (paste: Paste): string => {
        console.log(paste);
        let langs: string[] = [];
        for (const pasty of paste.pasties) {
            if (!langs.some((l) => l === pasty.language)) {
                langs.push(pasty.language);
            }
        }

        return langs.slice(0, 3).join(", ");
    };

    const fetchPastes = async (page: number) => {
        const res = await fetch(
            `${PUBLIC_API_BASE}/users/${data.user.username}/pastes?page=${page}&page_size=5`,
            {
                method: "get",
                credentials: "include"
            }
        );

        if (!res.ok) return;

        if (res.ok) {
            data.pastes = await res.json();
        }
    };

    const onPrevPage = async () => {
        if (data.pastes.currentPage === 0) return;

        await fetchPastes(data.pastes.currentPage - 1);
    };

    const onNextPage = async () => {
        if (data.pastes.currentPage === data.pastes.totalPages - 1) return;

        await fetchPastes(data.pastes.currentPage + 1);
    };
</script>

<svelte:head>
    <title>pastemyst | {data.user.username}</title>
</svelte:head>

<div class="flex sm-row">
    <section class="user-header flex sm-col center">
        <img
            class="avatar"
            src="{PUBLIC_API_BASE}/images/{data.user.avatarId}"
            alt="${data.user.username}'s avatar"
        />

        <div class="username flex col">
            <div class="flex row center username-top">
                <h2>{data.user.username}</h2>

                <div class="badges flex row center">
                    {#if data.user.contributor}
                        <div use:tooltip aria-label="contributor" class="flex">
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                viewBox="0 0 16 16"
                                class="icon contributor"
                            >
                                <title>Rocket Icon</title>
                                <path
                                    fill="currentColor"
                                    fill-rule="evenodd"
                                    d="M14.064 0a8.75 8.75 0 00-6.187 2.563l-.459.458c-.314.314-.616.641-.904.979H3.31a1.75 1.75 0 00-1.49.833L.11 7.607a.75.75 0 00.418 1.11l3.102.954c.037.051.079.1.124.145l2.429 2.428c.046.046.094.088.145.125l.954 3.102a.75.75 0 001.11.418l2.774-1.707a1.75 1.75 0 00.833-1.49V9.485c.338-.288.665-.59.979-.904l.458-.459A8.75 8.75 0 0016 1.936V1.75A1.75 1.75 0 0014.25 0h-.186zM10.5 10.625c-.088.06-.177.118-.266.175l-2.35 1.521.548 1.783 1.949-1.2a.25.25 0 00.119-.213v-2.066zM3.678 8.116L5.2 5.766c.058-.09.117-.178.176-.266H3.309a.25.25 0 00-.213.119l-1.2 1.95 1.782.547zm5.26-4.493A7.25 7.25 0 0114.063 1.5h.186a.25.25 0 01.25.25v.186a7.25 7.25 0 01-2.123 5.127l-.459.458a15.21 15.21 0 01-2.499 2.02l-2.317 1.5-2.143-2.143 1.5-2.317a15.25 15.25 0 012.02-2.5l.458-.458h.002zM12 5a1 1 0 11-2 0 1 1 0 012 0zm-8.44 9.56a1.5 1.5 0 10-2.12-2.12c-.734.73-1.047 2.332-1.15 3.003a.23.23 0 00.265.265c.671-.103 2.273-.416 3.005-1.148z"
                                />
                            </svg>
                        </div>
                    {/if}

                    {#if data.user.supporter}
                        <div
                            use:tooltip
                            aria-label="supporter for {data.user.supporter} months"
                            class="flex"
                        >
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                viewBox="0 0 16 16"
                                class="icon supporter"
                            >
                                <title>Heart Icon</title>
                                <path
                                    fill="currentColor"
                                    fill-rule="evenodd"
                                    d="M7.655 14.916L8 14.25l.345.666a.752.752 0 01-.69 0zm0 0L8 14.25l.345.666.002-.001.006-.003.018-.01a7.643 7.643 0 00.31-.17 22.08 22.08 0 003.433-2.414C13.956 10.731 16 8.35 16 5.5 16 2.836 13.914 1 11.75 1 10.203 1 8.847 1.802 8 3.02 7.153 1.802 5.797 1 4.25 1 2.086 1 0 2.836 0 5.5c0 2.85 2.045 5.231 3.885 6.818a22.075 22.075 0 003.744 2.584l.018.01.006.003h.002z"
                                />
                            </svg>
                        </div>
                    {/if}

                    {#if data.isCurrentUser}
                        <a
                            href="/settings/profile"
                            use:tooltip
                            aria-label="profile settings"
                            class="settings btn"
                        >
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                viewBox="0 0 16 16"
                                class="icon"
                            >
                                <title>Gear Icon</title>
                                <path
                                    fill="currentColor"
                                    fill-rule="evenodd"
                                    d="M7.429 1.525a6.593 6.593 0 011.142 0c.036.003.108.036.137.146l.289 1.105c.147.56.55.967.997 1.189.174.086.341.183.501.29.417.278.97.423 1.53.27l1.102-.303c.11-.03.175.016.195.046.219.31.41.641.573.989.014.031.022.11-.059.19l-.815.806c-.411.406-.562.957-.53 1.456a4.588 4.588 0 010 .582c-.032.499.119 1.05.53 1.456l.815.806c.08.08.073.159.059.19a6.494 6.494 0 01-.573.99c-.02.029-.086.074-.195.045l-1.103-.303c-.559-.153-1.112-.008-1.529.27-.16.107-.327.204-.5.29-.449.222-.851.628-.998 1.189l-.289 1.105c-.029.11-.101.143-.137.146a6.613 6.613 0 01-1.142 0c-.036-.003-.108-.037-.137-.146l-.289-1.105c-.147-.56-.55-.967-.997-1.189a4.502 4.502 0 01-.501-.29c-.417-.278-.97-.423-1.53-.27l-1.102.303c-.11.03-.175-.016-.195-.046a6.492 6.492 0 01-.573-.989c-.014-.031-.022-.11.059-.19l.815-.806c.411-.406.562-.957.53-1.456a4.587 4.587 0 010-.582c.032-.499-.119-1.05-.53-1.456l-.815-.806c-.08-.08-.073-.159-.059-.19a6.44 6.44 0 01.573-.99c.02-.029.086-.075.195-.045l1.103.303c.559.153 1.112.008 1.529-.27.16-.107.327-.204.5-.29.449-.222.851-.628.998-1.189l.289-1.105c.029-.11.101-.143.137-.146zM8 0c-.236 0-.47.01-.701.03-.743.065-1.29.615-1.458 1.261l-.29 1.106c-.017.066-.078.158-.211.224a5.994 5.994 0 00-.668.386c-.123.082-.233.09-.3.071L3.27 2.776c-.644-.177-1.392.02-1.82.63a7.977 7.977 0 00-.704 1.217c-.315.675-.111 1.422.363 1.891l.815.806c.05.048.098.147.088.294a6.084 6.084 0 000 .772c.01.147-.038.246-.088.294l-.815.806c-.474.469-.678 1.216-.363 1.891.2.428.436.835.704 1.218.428.609 1.176.806 1.82.63l1.103-.303c.066-.019.176-.011.299.071.213.143.436.272.668.386.133.066.194.158.212.224l.289 1.106c.169.646.715 1.196 1.458 1.26a8.094 8.094 0 001.402 0c.743-.064 1.29-.614 1.458-1.26l.29-1.106c.017-.066.078-.158.211-.224a5.98 5.98 0 00.668-.386c.123-.082.233-.09.3-.071l1.102.302c.644.177 1.392-.02 1.82-.63.268-.382.505-.789.704-1.217.315-.675.111-1.422-.364-1.891l-.814-.806c-.05-.048-.098-.147-.088-.294a6.1 6.1 0 000-.772c-.01-.147.039-.246.088-.294l.814-.806c.475-.469.679-1.216.364-1.891a7.992 7.992 0 00-.704-1.218c-.428-.609-1.176-.806-1.82-.63l-1.103.303c-.066.019-.176.011-.299-.071a5.991 5.991 0 00-.668-.386c-.133-.066-.194-.158-.212-.224L10.16 1.29C9.99.645 9.444.095 8.701.031A8.094 8.094 0 008 0zm1.5 8a1.5 1.5 0 11-3 0 1.5 1.5 0 013 0zM11 8a3 3 0 11-6 0 3 3 0 016 0z"
                                />
                            </svg>
                        </a>

                        <a
                            href="{PUBLIC_API_BASE}/auth/logout"
                            use:tooltip
                            aria-label="log out"
                            class="logout btn btn-danger"
                        >
                            <svg
                                xmlns="http://www.w3.org/2000/svg"
                                viewBox="0 0 16 16"
                                class="icon"
                            >
                                <title>Sign Out Icon</title>
                                <path
                                    fill="currentColor"
                                    fill-rule="evenodd"
                                    d="M2 2.75C2 1.784 2.784 1 3.75 1h2.5a.75.75 0 010 1.5h-2.5a.25.25 0 00-.25.25v10.5c0 .138.112.25.25.25h2.5a.75.75 0 010 1.5h-2.5A1.75 1.75 0 012 13.25V2.75zm10.44 4.5H6.75a.75.75 0 000 1.5h5.69l-1.97 1.97a.75.75 0 101.06 1.06l3.25-3.25a.75.75 0 000-1.06l-3.25-3.25a.75.75 0 10-1.06 1.06l1.97 1.97z"
                                />
                            </svg>
                        </a>
                    {/if}
                </div>
            </div>

            <p class="joined" use:tooltip aria-label={new Date(data.user.createdAt).toString()}>
                joined: {data.relativeJoined}
            </p>
        </div>
    </section>

    <section class="public-pastes">
        <h3>public pastes</h3>

        {#if data.pastes.items.length === 0}
            <p class="no-public-pastes">{data.user.username} doesn't have any public pastes yet.</p>
        {:else}
            {#each data.pastes.items as paste}
                <a href="/{paste.id}" class="paste btn">
                    <div class="flex row center space-between">
                        <p class="title">{paste.title === "" ? "(untitled)" : paste.title}</p>

                        <div>
                            {#if paste.private}
                                <div use:tooltip aria-label="private" class="flex">
                                    <svg
                                        xmlns="http://www.w3.org/2000/svg"
                                        viewBox="0 0 16 16"
                                        class="icon"
                                    >
                                        <title>Lock Closed Icon</title>
                                        <path
                                            fill="currentColor"
                                            fill-rule="evenodd"
                                            d="M4 4v2h-.25A1.75 1.75 0 002 7.75v5.5c0 .966.784 1.75 1.75 1.75h8.5A1.75 1.75 0 0014 13.25v-5.5A1.75 1.75 0 0012.25 6H12V4a4 4 0 10-8 0zm6.5 2V4a2.5 2.5 0 00-5 0v2h5zM12 7.5h.25a.25.25 0 01.25.25v5.5a.25.25 0 01-.25.25h-8.5a.25.25 0 01-.25-.25v-5.5a.25.25 0 01.25-.25H12z"
                                        />
                                    </svg>
                                </div>
                            {/if}
                        </div>
                    </div>

                    <div>
                        <!-- prettier-ignore -->
                        <span use:tooltip aria-label={new Date(paste.createdAt).toString()}>{moment(paste.createdAt).fromNow()}</span>

                        {#if paste.expiresIn !== ExpiresIn.never}
                            <!-- prettier-ignore -->
                            <span use:tooltip aria-label={new Date(paste.deletesAt).toString()}> - expires {moment(paste.deletesAt).fromNow()}</span>
                        {/if}
                    </div>

                    <div>
                        <span>{getPasteLangs(paste)}</span>
                    </div>
                </a>
            {/each}

            {#if data.pastes.totalPages > 1}
                <div class="pager flex row center">
                    <button
                        class="btn"
                        disabled={data.pastes.currentPage === 0}
                        on:click={onPrevPage}
                    >
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" class="icon">
                            <title>Chevron Left</title>
                            <path
                                fill="currentColor"
                                fill-rule="evenodd"
                                d="M9.78 12.78a.75.75 0 01-1.06 0L4.47 8.53a.75.75 0 010-1.06l4.25-4.25a.75.75 0 011.06 1.06L6.06 8l3.72 3.72a.75.75 0 010 1.06z"
                            />
                        </svg>
                    </button>
                    <span>{data.pastes.currentPage + 1}/{data.pastes.totalPages}</span>
                    <button class="btn" disabled={!data.pastes.hasNextPage} on:click={onNextPage}>
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 16 16" class="icon">
                            <title>Chevron Right</title>
                            <path
                                fill="currentColor"
                                fill-rule="evenodd"
                                d="M6.22 3.22a.75.75 0 011.06 0l4.25 4.25a.75.75 0 010 1.06l-4.25 4.25a.75.75 0 01-1.06-1.06L9.94 8 6.22 4.28a.75.75 0 010-1.06z"
                            />
                        </svg>
                    </button>
                </div>
            {/if}
        {/if}
    </section>
</div>

<style lang="scss">
    .user-header {
        margin-bottom: 0;
        height: 100%;

        .avatar {
            display: inline-block;
            border-radius: $border-radius;
            max-width: 16%;
            height: auto;
            margin-right: 1rem;
        }

        .username {
            width: 100%;

            h2 {
                margin: 0;
                margin-right: 0.5rem;
                font-size: $fs-large;
                word-break: break-word;
                font-weight: normal;
            }

            .username-top {
                width: 100%;
            }

            .badges {
                flex-grow: 1;

                .icon {
                    margin-right: 0.75rem;

                    &.contributor {
                        color: var(--color-secondary);
                    }

                    &.supporter {
                        color: var(--color-pink);
                    }
                }

                .settings {
                    margin-left: auto;

                    .icon {
                        margin: 0;
                    }
                }

                .logout {
                    margin-left: 0.5rem;

                    .icon {
                        margin: 0;
                        color: var(--color-danger);
                    }
                }
            }

            .joined {
                font-size: $fs-small;
                color: var(--color-bg3);
                margin: 0;
                margin-top: 0.25rem;
            }
        }
    }

    .public-pastes {
        flex-grow: 1;
        height: 100%;

        h3 {
            font-weight: normal;
            font-size: $fs-medium;
            margin: 0;
            margin-bottom: 1rem;
        }

        .no-public-pastes {
            text-align: center;
            margin: 0;
            font-size: $fs-normal;
        }

        .paste {
            display: block;
            background-color: var(--color-bg);
            margin-top: 1rem;
            border-radius: $border-radius;
            padding: 0.5rem;
            text-decoration: none;
            font-size: $fs-medium;
            border: 1px solid var(--color-bg2);
            color: var(--color-primary);

            &:hover {
                color: var(--color-secondary);
                background-color: var(--color-bg2);
                border-color: var(--color-bg3);
            }

            &:focus {
                color: var(--color-secondary);
                background-color: var(--color-bg2);
                border-color: var(--color-primary);
            }

            p {
                margin: 0;
            }

            .icon {
                color: var(--color-bg3);
            }

            span {
                font-size: $fs-small;
                color: var(--color-bg3);
            }
        }

        .pager {
            justify-content: center;
            margin-top: 1rem;
            font-size: $fs-normal;

            span {
                margin: 0 0.5rem;
            }

            button {
                .icon {
                    max-width: 18px;
                }
            }
        }
    }

    @media screen and (min-width: $break-med) {
        .user-header {
            min-width: 256px;
            max-width: 33.33%;
            margin-right: 2rem;

            .avatar {
                max-width: 100%;
                margin-right: 0;
                margin-bottom: 1rem;
            }
        }
    }
</style>
