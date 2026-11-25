import { Component, type ReactNode } from "react";

interface Props {
  children: ReactNode;
}
interface State {
  error: Error | null;
}

/** Ловить помилки завантаження деталей (відхилений проміс у use()). */
export default class ErrorBoundary extends Component<Props, State> {
  state: State = { error: null };

  static getDerivedStateFromError(error: Error): State {
    return { error };
  }

  render() {
    if (this.state.error) {
      return <div className="details error-text">Помилка: {this.state.error.message}</div>;
    }
    return this.props.children;
  }
}
