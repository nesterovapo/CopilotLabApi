# Blazor Frontend Guidelines

## Component Structure

- One component per file
- Components in PascalCase: `UserList.razor`, `ProductCard.razor`
- Component parameters use `[Parameter]` attribute
- Keep @code block at the bottom

## Naming Conventions

- Components: **PascalCase**
- CSS classes: **kebab-case** (e.g., `user-card`, `product-list`)
- JavaScript interop: **camelCase**

## State Management

- Use cascading parameters for shared state
- Avoid static state
- Use EventCallback for child-to-parent communication

## API Calls

- Use HttpClient injected via DI
- Handle loading and error states
- Show user-friendly error messages
- Use try-catch for all HTTP calls

## Styling

- Use Tailwind utility classes where possible
- Keep inline styles minimal
- Component-specific styles in `ComponentName.razor.css`

## Best Practices

- Use `@bind` for two-way binding
- Implement `IDisposable` when subscribing to events
- Async operations in `OnInitializedAsync()`
- Use `StateHasChanged()` sparingly
