import 'package:flutter/material.dart';

void showError(BuildContext ctx, Object e) {
  ScaffoldMessenger.of(ctx).showSnackBar(
    SnackBar(content: Text(e.toString()), backgroundColor: Colors.red),
  );
}

void showOk(BuildContext ctx, String msg) {
  ScaffoldMessenger.of(ctx).showSnackBar(SnackBar(content: Text(msg)));
}

Future<bool> confirm(BuildContext ctx, String title, String content) async {
  final ok = await showDialog<bool>(
    context: ctx,
    builder: (_) => AlertDialog(
      title: Text(title),
      content: Text(content),
      actions: [
        TextButton(
          onPressed: () => Navigator.pop(ctx, false),
          child: const Text('Hủy'),
        ),
        FilledButton(
          onPressed: () => Navigator.pop(ctx, true),
          child: const Text('Đồng ý'),
        ),
      ],
    ),
  );
  return ok ?? false;
}
