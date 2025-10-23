class AppSession {
  static final AppSession _inst = AppSession._();
  AppSession._();
  factory AppSession() => _inst;

  String? username;
  String? displayRole; // ví dụ: Bác sĩ
  String? role; // "Admin" | "User"
  bool get isAdmin => role == 'Admin';

  void loadFromLogin(Map<String, dynamic> data) {
    username = data['username'] as String?;
    displayRole = data['displayRole'] as String?;
    role = data['role'] as String?;
  }

  void clear() {
    username = null;
    displayRole = null;
    role = null;
  }
}
